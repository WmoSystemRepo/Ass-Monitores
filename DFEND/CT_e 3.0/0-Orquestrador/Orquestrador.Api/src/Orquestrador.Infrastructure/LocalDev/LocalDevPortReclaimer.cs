using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Orquestrador.Infrastructure.LocalDev;

/// <summary>
/// Descobre e encerra o(s) processo(s) que ocupam uma porta TCP local (órfãos LocalDev).
/// Cobre IPv4 + IPv6 e fallback via netstat (Angular costuma escutar em [::]).
/// </summary>
internal static class LocalDevPortReclaimer
{
    /// <summary>Garante porta livre: mata listeners (IPv4/IPv6/netstat) até soltar o socket.</summary>
    /// <returns>true se a porta ficou livre.</returns>
    public static bool TryEnsurePortFree(int port, out string? detail)
    {
        detail = null;
        if (port <= 0)
        {
            detail = "porta inválida";
            return false;
        }

        if (GetListeningPids(port).Count == 0 && !IsPortListening(port))
        {
            detail = $"nenhum listener na porta {port}";
            return true;
        }

        var killed = new List<int>();
        for (var attempt = 0; attempt < 4; attempt++)
        {
            var pids = GetListeningPids(port);
            if (pids.Count == 0 && !IsPortListening(port))
            {
                break;
            }

            var any = false;
            foreach (var pid in pids)
            {
                if (pid <= 0)
                {
                    continue;
                }

                if (pid == Environment.ProcessId)
                {
                    detail = $"porta {port} é do próprio Orquestrador (pid {pid})";
                    return false;
                }

                if (TryKillPid(pid, out _))
                {
                    killed.Add(pid);
                    any = true;
                }
            }

            if (!any)
            {
                break;
            }

            Thread.Sleep(400);
        }

        var free = GetListeningPids(port).Count == 0 && !IsPortListening(port);
        detail = killed.Count == 0
            ? (free ? $"porta {port} já livre" : $"não foi possível liberar porta {port}")
            : $"encerrado pid(s) {string.Join(", ", killed.Distinct())}" +
              (free ? $" (porta {port})" : $"; ainda ocupada");
        return free;
    }

    /// <summary>Compat: true só se matou alguém e a porta ficou livre.</summary>
    public static bool TryKillListenerOnPort(int port, out string? detail)
    {
        var before = GetListeningPids(port);
        var ok = TryEnsurePortFree(port, out detail);
        return ok && before.Count > 0;
    }

    public static bool TryGetPortFromUrl(string? url, out int port)
    {
        port = 0;
        if (string.IsNullOrWhiteSpace(url) || !Uri.TryCreate(url, UriKind.Absolute, out var uri))
        {
            return false;
        }

        port = uri.IsDefaultPort
            ? (string.Equals(uri.Scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase) ? 443 : 80)
            : uri.Port;
        return port > 0;
    }

    public static bool IsPortListening(int port)
    {
        try
        {
            return IPGlobalProperties.GetIPGlobalProperties()
                .GetActiveTcpListeners()
                .Any(ep => ep.Port == port);
        }
        catch
        {
            return GetListeningPids(port).Count > 0;
        }
    }

    private static HashSet<int> GetListeningPids(int port)
    {
        var pids = new HashSet<int>();
        CollectFromTcpTable(port, AF_INET, pids);
        CollectFromTcpTable(port, AF_INET6, pids);
        CollectFromNetstat(port, pids);
        pids.Remove(0);
        return pids;
    }

    private static void CollectFromTcpTable(int port, int addressFamily, HashSet<int> pids)
    {
        var bufferSize = 0;
        GetExtendedTcpTable(IntPtr.Zero, ref bufferSize, true, addressFamily, TCP_TABLE_OWNER_PID_LISTENER, 0);
        if (bufferSize <= 0)
        {
            return;
        }

        var buffer = Marshal.AllocHGlobal(bufferSize);
        try
        {
            var result = GetExtendedTcpTable(
                buffer,
                ref bufferSize,
                true,
                addressFamily,
                TCP_TABLE_OWNER_PID_LISTENER,
                0);
            if (result != 0)
            {
                return;
            }

            var rowCount = Marshal.ReadInt32(buffer);
            var rowPtr = IntPtr.Add(buffer, 4);
            // IPv4 row vs IPv6 row sizes differ; use family-specific layouts.
            if (addressFamily == AF_INET)
            {
                var rowSize = Marshal.SizeOf<MIB_TCPROW_OWNER_PID>();
                for (var i = 0; i < rowCount; i++)
                {
                    var row = Marshal.PtrToStructure<MIB_TCPROW_OWNER_PID>(rowPtr);
                    var localPort = (ushort)IPAddress.NetworkToHostOrder((short)row.localPort);
                    if (localPort == port)
                    {
                        pids.Add(row.owningPid);
                    }

                    rowPtr = IntPtr.Add(rowPtr, rowSize);
                }
            }
            else
            {
                var rowSize = Marshal.SizeOf<MIB_TCP6ROW_OWNER_PID>();
                for (var i = 0; i < rowCount; i++)
                {
                    var row = Marshal.PtrToStructure<MIB_TCP6ROW_OWNER_PID>(rowPtr);
                    var localPort = (ushort)IPAddress.NetworkToHostOrder((short)row.localPort);
                    if (localPort == port)
                    {
                        pids.Add(row.owningPid);
                    }

                    rowPtr = IntPtr.Add(rowPtr, rowSize);
                }
            }
        }
        finally
        {
            Marshal.FreeHGlobal(buffer);
        }
    }

    private static void CollectFromNetstat(int port, HashSet<int> pids)
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        try
        {
            using var proc = Process.Start(new ProcessStartInfo
            {
                FileName = "netstat",
                Arguments = "-ano -p tcp",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            });
            if (proc is null)
            {
                return;
            }

            var output = proc.StandardOutput.ReadToEnd();
            proc.WaitForExit(8000);
            //  TCP    0.0.0.0:4200    0.0.0.0:0    LISTENING    12345
            //  TCP    [::]:4200       [::]:0       LISTENING    12345
            var re = new Regex(
                $@":{port}\s+\S+\s+LISTENING\s+(\d+)\s*$",
                RegexOptions.IgnoreCase | RegexOptions.Multiline);
            foreach (Match m in re.Matches(output))
            {
                if (int.TryParse(m.Groups[1].Value, out var pid) && pid > 0)
                {
                    pids.Add(pid);
                }
            }
        }
        catch
        {
            // ignore
        }
    }

    private static bool TryKillPid(int pid, out string? detail)
    {
        detail = null;
        try
        {
            using var killer = Process.Start(new ProcessStartInfo
            {
                FileName = "taskkill",
                Arguments = $"/PID {pid} /T /F",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            });
            killer?.WaitForExit(8000);
            detail = $"encerrado pid {pid}";
            return true;
        }
        catch (Exception ex)
        {
            detail = $"falha ao matar pid {pid}: {ex.Message}";
            return false;
        }
    }

    private const int AF_INET = 2;
    private const int AF_INET6 = 23;
    private const int TCP_TABLE_OWNER_PID_LISTENER = 3;

    [StructLayout(LayoutKind.Sequential)]
    private struct MIB_TCPROW_OWNER_PID
    {
        public uint state;
        public uint localAddr;
        public uint localPort;
        public uint remoteAddr;
        public uint remotePort;
        public int owningPid;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct MIB_TCP6ROW_OWNER_PID
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] localAddr;
        public uint localScopeId;
        public uint localPort;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] remoteAddr;
        public uint remoteScopeId;
        public uint remotePort;
        public uint state;
        public int owningPid;
    }

    [DllImport("iphlpapi.dll", SetLastError = true)]
    private static extern uint GetExtendedTcpTable(
        IntPtr pTcpTable,
        ref int dwOutBufLen,
        bool sort,
        int ipVersion,
        int tblClass,
        uint reserved);
}
