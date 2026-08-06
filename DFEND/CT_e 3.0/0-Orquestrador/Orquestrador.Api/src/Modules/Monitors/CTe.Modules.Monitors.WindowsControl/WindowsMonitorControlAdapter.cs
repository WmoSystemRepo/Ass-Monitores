using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;
using System.ServiceProcess;

namespace CTe.Modules.Monitors.WindowsControl;

/// <summary>
/// Controle Windows/DevHost genérico de um monitor (W3 — SDD Monitor Unificado). Cópia/adaptação de
/// 1-Receptor/Receptor.Api/src/Monitor.Infrastructure/Windows/WindowsServiceControllerAdapter.cs —
/// mesma técnica (PreferLocalProcess evita tocar no SCM em POC; fallback compila/inicia o DevHost),
/// generalizada para qualquer <see cref="MonitorControlOptions"/> em vez de só o Receptor.
/// Idempotente: já Running/Stopped conta como sucesso (mesma regra do ServiceControlService original).
/// </summary>
[SupportedOSPlatform("windows")]
[SuppressMessage("Interoperability", "CA1416", Justification = "Monitores CT-e são Windows-only (SCM + DevHost .NET Framework).")]
public sealed class WindowsMonitorControlAdapter
{
    private readonly MonitorControlOptions _options;
    private readonly string?[] _searchStarts;
    private readonly object _startLock = new();
    private bool? _scmInstalledCache;
    private DateTimeOffset _scmInstalledCacheAt = DateTimeOffset.MinValue;
    private static readonly TimeSpan ScmInstallCacheTtl = TimeSpan.FromMinutes(2);

    public WindowsMonitorControlAdapter(MonitorControlOptions options, params string?[] repoRootSearchStarts)
    {
        _options = options;
        _searchStarts = repoRootSearchStarts;
    }

    public ServiceControlResult GetStatus()
    {
        var serviceName = _options.WindowsServiceName;

        if (IsLocalHostRunning())
        {
            return new ServiceControlResult(true, "Running", $"{_options.DisplayName} ligado.");
        }

        if (_options.PreferLocalProcess)
        {
            // Status/poll NÃO compila DevHost — MSBuild no snapshot travava a UI (~1 min).
            if (TryFindExistingExe(out var exePath, out var resolveError))
            {
                return new ServiceControlResult(
                    true,
                    "Stopped",
                    $"{_options.DisplayName} desligado. Pronto para ligar ({RepoRootResolver.ToPortableRelative(exePath)}).");
            }

            return new ServiceControlResult(
                false,
                "NotFound",
                resolveError ?? $"{_options.DisplayName} não disponível. Compile o DevHost (Debug).");
        }

        var scm = TryGetScmStatusSafe(serviceName);
        if (scm is not null)
        {
            return scm;
        }

        if (TryFindExistingExe(out var path, out _))
        {
            return new ServiceControlResult(
                true,
                "Stopped",
                $"SCM ausente; host POC disponível: {RepoRootResolver.ToPortableRelative(path)}");
        }

        return new ServiceControlResult(false, "NotFound", "Nem host POC nem Windows Service disponíveis.");
    }

    public ServiceControlResult Start()
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

            var serviceName = _options.WindowsServiceName;
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

    public ServiceControlResult Stop()
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
                        ? $"{_options.DisplayName} desligado. Detalhe técnico: host encerrado ({killed}); Executar=0."
                        : $"{_options.DisplayName} desligado. Detalhe técnico: nada em execução; Executar=0.");
            }

            var serviceName = _options.WindowsServiceName;
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
                    $"Host saiu imediatamente (exit={process.ExitCode}). Verifique {psi.WorkingDirectory}\\{Path.GetFileName(exePath)}.config");
            }

            return new ServiceControlResult(
                true,
                "Running",
                $"{_options.DisplayName} ligado (PID {process.Id}). Detalhe: {_options.ProcessName} — {exePath}");
        }
        catch (Exception ex)
        {
            return new ServiceControlResult(false, "Error", ex.Message);
        }
    }

    private void EnsureDevConfigBesideExe(string exePath)
    {
        var root = FindPackageRoot();
        if (root is null || string.IsNullOrWhiteSpace(_options.MonitoredService))
        {
            return;
        }

        var src = Path.Combine(
            root,
            $"dfend-cte-{_options.Domain}-windowsservices",
            _options.MonitoredService,
            "AppConfig",
            "Desenvolvimento",
            $"{_options.MonitoredService}.exe.config");
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

    private ServiceControlResult? TryGetScmStatusSafe(string serviceName)
    {
        if (string.IsNullOrWhiteSpace(serviceName) || !IsScmInstalled(serviceName))
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
        if (string.IsNullOrWhiteSpace(serviceName))
        {
            return false;
        }

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

    /// <summary>Só localiza o .exe existente — usado em GetStatus/poll (sem MSBuild).</summary>
    private bool TryFindExistingExe(out string exePath, out string? error)
    {
        exePath = string.Empty;
        error = null;

        if (string.IsNullOrWhiteSpace(_options.ExeRelativePath))
        {
            error = $"Monitors:{_options.ServiceId}:ExeRelativePath não configurado.";
            return false;
        }

        var root = FindPackageRoot();
        if (root is null)
        {
            error =
                $"Não achei a pasta {_options.PackageFolder}/ (precisa ter dfend-cte-{_options.Domain}-windowsservices + " +
                $"{_options.PackageFolder}/*.Api). Defina Monitors:{_options.ServiceId}:RootPath.";
            return false;
        }

        var full = Path.GetFullPath(Path.Combine(root, _options.ExeRelativePath));
        if (File.Exists(full))
        {
            exePath = full;
            return true;
        }

        // Fallback: builds antigos sob _artifacts (Directory.Build.props) antes da correção.
        foreach (var candidate in EnumerateDevHostExeFallbacks(root, full))
        {
            if (File.Exists(candidate))
            {
                exePath = candidate;
                return true;
            }
        }

        var devHostName = _options.ProcessName ?? Path.GetFileNameWithoutExtension(full);
        error = HostMissingMessage(full, null);
        return false;
    }

    private IEnumerable<string> EnumerateDevHostExeFallbacks(string packageRoot, string primaryExe)
    {
        var fileName = Path.GetFileName(primaryExe);
        var projectName = _options.ProcessName ?? Path.GetFileNameWithoutExtension(primaryExe);
        if (string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(projectName))
        {
            yield break;
        }

        var repoRoot = RepoRootResolver.FindRepoRoot(null, packageRoot);
        if (repoRoot is null)
        {
            foreach (var start in _searchStarts)
            {
                repoRoot = RepoRootResolver.FindRepoRoot(null, start);
                if (repoRoot is not null)
                {
                    break;
                }
            }
        }

        if (repoRoot is null)
        {
            yield break;
        }

        yield return Path.Combine(repoRoot, "_artifacts", "bin", projectName, "Debug", fileName);
        yield return Path.Combine(repoRoot, "_artifacts", "obj", projectName, "Debug", fileName);
        yield return Path.Combine(repoRoot, "_artifacts", "bin", projectName, "Debug", "net47", fileName);
    }

    /// <summary>Localiza o .exe; se faltar, tenta compilar o DevHost (somente no Start).</summary>
    private bool TryResolveExePath(out string exePath, out string? error)
    {
        if (TryFindExistingExe(out exePath, out error))
        {
            return true;
        }

        if (string.IsNullOrWhiteSpace(_options.ExeRelativePath))
        {
            return false;
        }

        var root = FindPackageRoot();
        if (root is null)
        {
            return false;
        }

        var full = Path.GetFullPath(Path.Combine(root, _options.ExeRelativePath));
        var devHostName = _options.ProcessName ?? Path.GetFileNameWithoutExtension(full);
        var csproj = FindDevHostCsproj(full, devHostName);
        string? buildError = null;
        if (csproj is null || !DevHostMsBuild.TryBuild(csproj, full, out buildError))
        {
            error = HostMissingMessage(full, buildError);
            exePath = string.Empty;
            return false;
        }

        exePath = full;
        error = null;
        return true;
    }

    private string? FindPackageRoot()
    {
        // Sempre ancora no processo (ContentRoot/BaseDirectory) — ignora RootPath de outro clone/usuário.
        var repoRoot = RepoRootResolver.FindRepoRoot(null, _searchStarts);
        return RepoRootResolver.ResolveConfiguredPackageRoot(
            _options.RootPath,
            repoRoot,
            _options.PackageFolder);
    }

    private string HostMissingMessage(string? absoluteExePath, string? detail)
    {
        var relative = !string.IsNullOrWhiteSpace(absoluteExePath)
            ? RepoRootResolver.ToPortableRelative(absoluteExePath)
            : Path.Combine(
                _options.PackageFolder ?? "engines",
                _options.ExeRelativePath ?? "*.DevHost.exe");
        var name = _options.ProcessName ?? "DevHost";
        var suffix = string.IsNullOrWhiteSpace(detail) ? string.Empty : $" ({detail})";
        return
            $"Host POC não encontrado: {relative}{suffix}. " +
            $"Compile tools\\{name} (Debug) ou rode tools\\build-devhosts.ps1 na pasta 0-Orquestrador deste clone.";
    }

    /// <summary>
    /// Localiza o .csproj do DevHost subindo a partir do caminho do .exe
    /// (ex.: …\X.DevHost\bin\Debug\X.exe → …\X.DevHost\X.csproj).
    /// </summary>
    private static string? FindDevHostCsproj(string exeFullPath, string devHostName)
    {
        if (string.IsNullOrWhiteSpace(devHostName))
        {
            return null;
        }

        var dir = Path.GetDirectoryName(exeFullPath);
        for (var i = 0; i < 4 && dir is not null; i++)
        {
            var candidate = Path.Combine(dir, $"{devHostName}.csproj");
            if (File.Exists(candidate))
            {
                return candidate;
            }

            dir = Path.GetDirectoryName(dir);
        }

        return null;
    }

    private bool IsLocalHostRunning()
    {
        try
        {
            var hostName = string.IsNullOrWhiteSpace(_options.ProcessName)
                ? _options.MonitoredService
                : _options.ProcessName;
            return !string.IsNullOrWhiteSpace(hostName) && Process.GetProcessesByName(hostName).Length > 0;
        }
        catch
        {
            return false;
        }
    }

    private int StopLocalProcesses()
    {
        var count = 0;
        var hostName = string.IsNullOrWhiteSpace(_options.ProcessName)
            ? _options.MonitoredService
            : _options.ProcessName;

        if (string.IsNullOrWhiteSpace(hostName))
        {
            return count;
        }

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
