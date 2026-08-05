using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Orquestrador.Infrastructure.LocalDev;

/// <summary>
/// Windows Job Object com KILL_ON_JOB_CLOSE: se o Orquestrador morrer
/// (Shift+F5, crash, kill), os filhos (Monitor.Api / npm) morrem junto e liberam DLLs/portas.
/// </summary>
internal sealed class LocalDevJobObject : IDisposable
{
    private IntPtr _handle;
    private bool _disposed;

    public LocalDevJobObject()
    {
        _handle = CreateJobObject(IntPtr.Zero, $"cte-orq-localdev-{Environment.ProcessId}");
        if (_handle == IntPtr.Zero)
        {
            throw new InvalidOperationException(
                $"CreateJobObject falhou (Win32={Marshal.GetLastWin32Error()}).");
        }

        var info = new JOBOBJECT_EXTENDED_LIMIT_INFORMATION
        {
            BasicLimitInformation = new JOBOBJECT_BASIC_LIMIT_INFORMATION
            {
                LimitFlags = JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE
            }
        };

        var length = Marshal.SizeOf<JOBOBJECT_EXTENDED_LIMIT_INFORMATION>();
        var ptr = Marshal.AllocHGlobal(length);
        try
        {
            Marshal.StructureToPtr(info, ptr, false);
            if (!SetInformationJobObject(
                    _handle,
                    JOBOBJECTINFOCLASS.JobObjectExtendedLimitInformation,
                    ptr,
                    (uint)length))
            {
                throw new InvalidOperationException(
                    $"SetInformationJobObject falhou (Win32={Marshal.GetLastWin32Error()}).");
            }
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }
    }

    public void Assign(Process process)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        if (process.HasExited)
        {
            return;
        }

        if (!AssignProcessToJobObject(_handle, process.Handle))
        {
            var err = Marshal.GetLastWin32Error();
            // ACCESS_DENIED (5) pode ocorrer se o processo já saiu ou está em outro job.
            if (err is not 5 and not 87)
            {
                throw new InvalidOperationException(
                    $"AssignProcessToJobObject falhou pid={process.Id} (Win32={err}).");
            }
        }
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        if (_handle != IntPtr.Zero)
        {
            CloseHandle(_handle);
            _handle = IntPtr.Zero;
        }
    }

    private const uint JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE = 0x00002000;

    private enum JOBOBJECTINFOCLASS
    {
        JobObjectExtendedLimitInformation = 9
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct JOBOBJECT_BASIC_LIMIT_INFORMATION
    {
        public long PerProcessUserTimeLimit;
        public long PerJobUserTimeLimit;
        public uint LimitFlags;
        public UIntPtr MinimumWorkingSetSize;
        public UIntPtr MaximumWorkingSetSize;
        public uint ActiveProcessLimit;
        public UIntPtr Affinity;
        public uint PriorityClass;
        public uint SchedulingClass;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct IO_COUNTERS
    {
        public ulong ReadOperationCount;
        public ulong WriteOperationCount;
        public ulong OtherOperationCount;
        public ulong ReadTransferCount;
        public ulong WriteTransferCount;
        public ulong OtherTransferCount;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct JOBOBJECT_EXTENDED_LIMIT_INFORMATION
    {
        public JOBOBJECT_BASIC_LIMIT_INFORMATION BasicLimitInformation;
        public IO_COUNTERS IoInfo;
        public UIntPtr ProcessMemoryLimit;
        public UIntPtr JobMemoryLimit;
        public UIntPtr PeakProcessMemoryUsed;
        public UIntPtr PeakJobMemoryUsed;
    }

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern IntPtr CreateJobObject(IntPtr lpJobAttributes, string? lpName);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool SetInformationJobObject(
        IntPtr hJob,
        JOBOBJECTINFOCLASS infoClass,
        IntPtr lpJobObjectInfo,
        uint cbJobObjectInfoLength);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool AssignProcessToJobObject(IntPtr hJob, IntPtr hProcess);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool CloseHandle(IntPtr hObject);
}
