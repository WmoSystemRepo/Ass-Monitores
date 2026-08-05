using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;
using System.ServiceProcess;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Monitor.Application.Abstractions;
using Monitor.Application.Services;

namespace Monitor.Infrastructure.Windows;

/// <summary>
/// Controle operacional do Analisador em POC.
/// Com PreferLocalProcess=true (padrão): NÃO consulta SCM — evita InvalidOperationException
/// (e o spam "Exceção gerada" no Visual Studio) quando o Windows Service não está instalado.
/// </summary>
[SupportedOSPlatform("windows")]
[SuppressMessage("Interoperability", "CA1416", Justification = "Monitor.Api POC é Windows-only.")]
public sealed class WindowsServiceControllerAdapter : IWindowsServiceController
{
    private readonly MonitorOptions _options;
    private readonly IHostEnvironment _env;
    private readonly object _startLock = new();
    private bool? _scmInstalledCache;
    private DateTimeOffset _scmInstalledCacheAt = DateTimeOffset.MinValue;
    private static readonly TimeSpan ScmInstallCacheTtl = TimeSpan.FromMinutes(2);

    public WindowsServiceControllerAdapter(IOptions<MonitorOptions> options, IHostEnvironment env)
    {
        _options = options.Value;
        _env = env;
    }

    public ServiceControlResult GetStatus(string serviceName)
    {
        if (IsLocalHostRunning())
        {
            return new ServiceControlResult(true, "Running", "Analisador CT-e ligado.");
        }

        // POC: não toca no SCM — elimina exceção de primeira chance no depurador
        if (_options.PreferLocalProcess)
        {
            if (TryResolveExePath(out var exePath, out var resolveError))
            {
                return new ServiceControlResult(
                    true,
                    "Stopped",
                    $"Analisador CT-e desligado. Pronto para ligar ({exePath}).");
            }

            return new ServiceControlResult(
                false,
                "NotFound",
                resolveError
                ?? "Analisador CT-e não disponível. Compile tools/Analisador.DevHost (Debug).");
        }

        var scm = TryGetScmStatusSafe(serviceName);
        if (scm is not null)
        {
            return scm;
        }

        if (TryResolveExePath(out var path, out _))
        {
            return new ServiceControlResult(true, "Stopped", $"SCM ausente; host POC disponível: {path}");
        }

        return new ServiceControlResult(false, "NotFound", "Nem host POC nem Windows Service disponíveis.");
    }

    public ServiceControlResult Start(string serviceName)
    {
        lock (_startLock)
        {
            if (IsLocalHostRunning())
            {
                return new ServiceControlResult(true, "Running", "Host POC já em execução.");
            }

            if (_options.PreferLocalProcess)
            {
                if (!TryResolveExePath(out var exePath, out var resolveError))
                {
                    return new ServiceControlResult(false, "NotFound", resolveError ?? "Host POC não encontrado.");
                }

                return StartLocalProcess(exePath);
            }

            var scm = TryGetScmStatusSafe(serviceName);
            if (scm is { Success: true }
                && (scm.Status.Equals("Running", StringComparison.OrdinalIgnoreCase)
                    || scm.Status.Equals("StartPending", StringComparison.OrdinalIgnoreCase)))
            {
                return scm;
            }

            if (scm is { Success: true } && IsScmInstalled(serviceName))
            {
                return StartScm(serviceName);
            }

            if (TryResolveExePath(out var hostPath, out var err))
            {
                return StartLocalProcess(hostPath);
            }

            return new ServiceControlResult(false, "NotFound", err ?? "Não foi possível iniciar.");
        }
    }

    public ServiceControlResult Stop(string serviceName)
    {
        lock (_startLock)
        {
            var killed = StopLocalProcesses();

            if (_options.PreferLocalProcess)
            {
                return new ServiceControlResult(
                    true,
                    "Stopped",
                    killed > 0
                        ? $"Analisador CT-e desligado. Detalhe técnico: host encerrado ({killed}); Executar=0."
                        : "Analisador CT-e desligado. Detalhe técnico: nada em execução; Executar=0.");
            }

            string? scmMessage = null;
            if (IsScmInstalled(serviceName))
            {
                try
                {
                    using var sc = new ServiceController(serviceName);
                    if (sc.Status is not ServiceControllerStatus.Stopped and not ServiceControllerStatus.StopPending)
                    {
                        sc.Stop();
                        sc.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(30));
                        scmMessage = "SCM parado.";
                    }
                    else
                    {
                        scmMessage = "SCM já parado.";
                    }
                }
                catch (Exception ex)
                {
                    scmMessage = ex.Message;
                }
            }

            var parts = new List<string>();
            if (killed > 0)
            {
                parts.Add($"Host POC encerrado ({killed}).");
            }

            if (scmMessage is not null)
            {
                parts.Add(scmMessage);
            }

            parts.Add("Executar=0.");
            return new ServiceControlResult(true, "Stopped", string.Join(" ", parts));
        }
    }

    private ServiceControlResult StartLocalProcess(string exePath)
    {
        try
        {
            EnsureDevConfigBesideExe(exePath);

            var psi = new ProcessStartInfo
            {
                FileName = exePath,
                WorkingDirectory = Path.GetDirectoryName(exePath)!,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            var process = Process.Start(psi);
            if (process is null)
            {
                return new ServiceControlResult(false, "Error", "Process.Start retornou null.");
            }

            Thread.Sleep(800);
            if (process.HasExited)
            {
                return new ServiceControlResult(
                    false,
                    "Error",
                    $"Host saiu imediatamente (exit={process.ExitCode}). Verifique {psi.WorkingDirectory}\\Analisador.DevHost.exe.config");
            }

            return new ServiceControlResult(
                true,
                "Running",
                $"Analisador CT-e ligado (PID {process.Id}). Detalhe: Analisador.DevHost — {exePath}");
        }
        catch (Exception ex)
        {
            return new ServiceControlResult(false, "Error", ex.Message);
        }
    }

    private void EnsureDevConfigBesideExe(string exePath)
    {
        var root = FindAnalisadorRoot();
        if (root is null)
        {
            return;
        }

        var src = Path.Combine(
            root,
            "dfend-cte-analisador-windowsservices",
            "DFEND_CTe_Analisador",
            "AppConfig",
            "Desenvolvimento",
            "DFEND_CTe_Analisador.exe.config");
        if (!File.Exists(src))
        {
            return;
        }

        File.Copy(src, exePath + ".config", overwrite: true);
    }

    private static ServiceControlResult StartScm(string serviceName)
    {
        try
        {
            using var sc = new ServiceController(serviceName);
            if (sc.Status is ServiceControllerStatus.Running or ServiceControllerStatus.StartPending)
            {
                return new ServiceControlResult(true, sc.Status.ToString(), "Já em execução (SCM).");
            }

            sc.Start();
            sc.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30));
            return new ServiceControlResult(true, sc.Status.ToString(), "Serviço iniciado via SCM.");
        }
        catch (Exception ex)
        {
            return new ServiceControlResult(false, "Error", ex.Message);
        }
    }

    /// <summary>
    /// Só cria ServiceController se o serviço existir na lista do SCM (sem throw por nome inexistente).
    /// </summary>
    private ServiceControlResult? TryGetScmStatusSafe(string serviceName)
    {
        if (!IsScmInstalled(serviceName))
        {
            return null;
        }

        try
        {
            using var sc = new ServiceController(serviceName);
            return new ServiceControlResult(true, sc.Status.ToString(), "Status via SCM.");
        }
        catch (Exception ex)
        {
            return new ServiceControlResult(false, "Error", ex.Message);
        }
    }

    private bool IsScmInstalled(string serviceName)
    {
        if (_scmInstalledCache is not null
            && DateTimeOffset.UtcNow - _scmInstalledCacheAt < ScmInstallCacheTtl)
        {
            return _scmInstalledCache.Value;
        }

        var installed = false;
        try
        {
            foreach (var sc in ServiceController.GetServices())
            {
                try
                {
                    if (sc.ServiceName.Equals(serviceName, StringComparison.OrdinalIgnoreCase))
                    {
                        installed = true;
                        break;
                    }
                }
                finally
                {
                    sc.Dispose();
                }
            }
        }
        catch
        {
            installed = false;
        }

        _scmInstalledCache = installed;
        _scmInstalledCacheAt = DateTimeOffset.UtcNow;
        return installed;
    }

    private bool TryResolveExePath(out string exePath, out string? error)
    {
        exePath = string.Empty;
        error = null;

        var relative = string.IsNullOrWhiteSpace(_options.AnalisadorExeRelativePath)
            ? @"tools\Analisador.DevHost\bin\Debug\Analisador.DevHost.exe"
            : _options.AnalisadorExeRelativePath;

        var root = FindAnalisadorRoot();
        if (root is null)
        {
            error =
                "Não achei a pasta 4-Analisador/ (precisa ter dfend-cte-analisador-windowsservices + Analisador.Api). " +
                "Se o Monitor.Api foi iniciado pelo Orquestrador (shadow), reinicie o Orquestrador após atualizar, " +
                "ou defina Monitor:AnalisadorRootPath.";
            return false;
        }

        var full = Path.GetFullPath(Path.Combine(root, relative));
        if (!File.Exists(full))
        {
            var csproj = Path.Combine(root, @"tools\Analisador.DevHost\Analisador.DevHost.csproj");
            if (!DevHostMsBuild.TryBuild(csproj, full, out var buildError))
            {
                error =
                    buildError
                    ?? $"Host POC não encontrado: {full}. Compile tools\\Analisador.DevHost (Debug) ou rode 0-Orquestrador\\tools\\build-devhosts.ps1.";
                return false;
            }
        }

        exePath = full;
        return true;
    }

    private string? FindAnalisadorRoot()
    {
        if (!string.IsNullOrWhiteSpace(_options.AnalisadorRootPath)
            && Directory.Exists(_options.AnalisadorRootPath))
        {
            return Path.GetFullPath(_options.AnalisadorRootPath);
        }

        foreach (var start in new[]
                 {
                     _env.ContentRootPath,
                     AppContext.BaseDirectory,
                     Directory.GetCurrentDirectory()
                 })
        {
            var dir = new DirectoryInfo(start);
            while (dir is not null)
            {
                var hasSvc = Directory.Exists(Path.Combine(dir.FullName, "dfend-cte-analisador-windowsservices"));
                var hasApi = Directory.Exists(Path.Combine(dir.FullName, "Analisador.Api"));
                if (hasSvc && hasApi)
                {
                    return dir.FullName;
                }

                dir = dir.Parent;
            }
        }

        return null;
    }

    private bool IsLocalHostRunning()
    {
        try
        {
            var hostName = string.IsNullOrWhiteSpace(_options.AnalisadorProcessName)
                ? "Analisador.DevHost"
                : _options.AnalisadorProcessName;
            return Process.GetProcessesByName(hostName).Length > 0;
        }
        catch
        {
            return false;
        }
    }

    private int StopLocalProcesses()
    {
        var count = 0;
        var hostName = string.IsNullOrWhiteSpace(_options.AnalisadorProcessName)
            ? "Analisador.DevHost"
            : _options.AnalisadorProcessName;

        foreach (var p in Process.GetProcessesByName(hostName))
        {
            try
            {
                p.Kill(entireProcessTree: true);
                p.WaitForExit(15_000);
                count++;
            }
            catch
            {
                // ignore
            }
            finally
            {
                p.Dispose();
            }
        }

        return count;
    }
}
