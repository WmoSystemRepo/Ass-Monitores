using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orquestrador.Application.Abstractions;
using Orquestrador.Application.Options;

namespace Orquestrador.Infrastructure.LocalDev;

/// <summary>
/// Implementação de <see cref="IMonitorProcessLauncher"/> para Development.
/// ProjectPath/FrontendProjectPath são relativos à raiz CT_e (descoberta automática;
/// prefixo absoluto da máquina é irrelevante).
/// </summary>
public sealed class MonitorProcessLauncher : IMonitorProcessLauncher, IDisposable
{
    private readonly IOptions<OrchestratorOptions> _options;
    private readonly IHostEnvironment _env;
    private readonly ILogger<MonitorProcessLauncher> _logger;
    private readonly ConcurrentDictionary<string, int> _launchedApiPids = new(StringComparer.OrdinalIgnoreCase);
    private readonly ConcurrentDictionary<string, int> _launchedFrontPids = new(StringComparer.OrdinalIgnoreCase);
    private readonly ConcurrentDictionary<string, byte> _apiLocks = new(StringComparer.OrdinalIgnoreCase);
    private readonly ConcurrentDictionary<string, byte> _frontLocks = new(StringComparer.OrdinalIgnoreCase);
    private readonly HttpClient _healthClient;
    private readonly object _jobLock = new();
    private LocalDevJobObject? _job;
    private bool _disposed;

    public MonitorProcessLauncher(
        IOptions<OrchestratorOptions> options,
        IHostEnvironment env,
        ILogger<MonitorProcessLauncher> logger)
    {
        _options = options;
        _env = env;
        _logger = logger;
        _healthClient = new HttpClient { Timeout = TimeSpan.FromSeconds(5) };
    }

    /// <inheritdoc />
    public Task<bool> IsApiReadyAsync(string baseUrl, CancellationToken ct)
    {
        return IsHttpOkAsync(Combine(baseUrl, "/health/ready"), ct);
    }

    /// <inheritdoc />
    public Task<bool> IsFrontendReachableAsync(string frontendUrl, CancellationToken ct)
    {
        return IsAngularDevServerReadyAsync(frontendUrl.Trim().TrimEnd('/'), ct);
    }

    /// <inheritdoc />
    public async Task<bool> EnsureApiReadyAsync(OrchestratorSystemOptions system, CancellationToken ct)
    {
        var apiBase = system.ResolveApiBase();
        if (string.IsNullOrWhiteSpace(apiBase))
        {
            return false;
        }

        if (await IsApiReadyAsync(apiBase, ct).ConfigureAwait(false))
        {
            return true;
        }

        if (!_env.IsDevelopment())
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(system.ProjectPath))
        {
            _logger.LogWarning(
                "Monitor {Id} offline e sem ProjectPath — não é possível auto-start.",
                system.Id);
            return false;
        }

        var timeoutSeconds = Math.Max(5, _options.Value.LocalDev.StartupTimeoutSeconds);

        if (!_apiLocks.TryAdd(system.Id, 0))
        {
            return await WaitUntilAsync(
                () => IsApiReadyAsync(apiBase, ct),
                timeoutSeconds,
                ct).ConfigureAwait(false);
        }

        try
        {
            if (await IsApiReadyAsync(apiBase, ct).ConfigureAwait(false))
            {
                return true;
            }

            // Porta ocupada sem /health/ready = órfão de sessão anterior (Shift+F5).
            TryReclaimOrphanPort(apiBase, $"api:{system.Id}");

            if (await IsApiReadyAsync(apiBase, ct).ConfigureAwait(false))
            {
                return true;
            }

            if (!TryResolveFilePath(system.ProjectPath, out var projectPath, out var resolveError))
            {
                _logger.LogWarning(
                    "Não foi possível resolver ProjectPath do {Id}: {Error}",
                    system.Id,
                    resolveError);
                return false;
            }

            if (!TryStartMonitorApi(system, projectPath, out var startError))
            {
                _logger.LogWarning("Falha ao iniciar Monitor.Api {Id}: {Error}", system.Id, startError);
                return false;
            }

            _logger.LogInformation(
                "Monitor.Api {Id} iniciado (shadow %TEMP%, sem lock no bin). Aguardando /health/ready em {BaseUrl}…",
                system.Id,
                apiBase);

            var ready = await WaitUntilAsync(
                () => IsApiReadyAsync(apiBase, ct),
                timeoutSeconds,
                ct,
                elapsed => _logger.LogInformation(
                    "Aguardando API {Id} em {BaseUrl}… ({Elapsed}s)",
                    system.Id,
                    apiBase,
                    elapsed)).ConfigureAwait(false);

            if (!ready)
            {
                _logger.LogWarning(
                    "Monitor.Api {Id} não ficou ready em {Timeout}s (processo pode ter subido sem SQL).",
                    system.Id,
                    timeoutSeconds);
            }

            return ready;
        }
        finally
        {
            _apiLocks.TryRemove(system.Id, out _);
        }
    }

    /// <inheritdoc />
    public async Task<bool> EnsureFrontendAsync(OrchestratorSystemOptions system, CancellationToken ct)
    {
        var frontendUrl = system.ResolveFrontendUrl();
        if (string.IsNullOrWhiteSpace(frontendUrl))
        {
            return false;
        }

        system.FrontendUrl ??= frontendUrl;

        if (await IsFrontendReachableAsync(frontendUrl, ct).ConfigureAwait(false))
        {
            return true;
        }

        if (!_env.IsDevelopment())
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(system.FrontendProjectPath))
        {
            _logger.LogWarning(
                "Frontend {Id} offline e sem FrontendProjectPath — não é possível auto-start.",
                system.Id);
            return false;
        }

        var timeoutSeconds = Math.Max(60, _options.Value.LocalDev.StartupTimeoutSeconds);

        if (!_frontLocks.TryAdd(system.Id, 0))
        {
            return await WaitUntilAsync(
                () => IsFrontendReachableAsync(frontendUrl, ct),
                timeoutSeconds,
                ct).ConfigureAwait(false);
        }

        try
        {
            if (await IsFrontendReachableAsync(frontendUrl, ct).ConfigureAwait(false))
            {
                return true;
            }

            // Porta já em uso (ex.: start-chain-fronts compilando) → ESPERAR ficar ready.
            // NÃO matar o listener aqui: reclaim no meio do build derruba o Angular (ex. Carga :4260)
            // e a UI fica em "Abrindo…" para sempre.
            if (LocalDevPortReclaimer.TryGetPortFromUrl(frontendUrl, out var busyPort) &&
                LocalDevPortReclaimer.IsPortListening(busyPort))
            {
                _logger.LogInformation(
                    "Frontend {Id}: porta {Port} já escutando — aguardando compile (sem reclaim)…",
                    system.Id,
                    busyPort);

                var readyBusy = await WaitUntilAsync(
                    () => IsFrontendReachableAsync(frontendUrl, ct),
                    timeoutSeconds,
                    ct,
                    elapsed => _logger.LogInformation(
                        "Aguardando Angular {Id} (porta já ocupada) em {Url}… ({Elapsed}s)",
                        system.Id,
                        frontendUrl,
                        elapsed)).ConfigureAwait(false);

                if (readyBusy)
                {
                    return true;
                }

                _logger.LogWarning(
                    "Frontend {Id}: timeout na porta {Port} — liberando órfão e reiniciando",
                    system.Id,
                    busyPort);
                TryReclaimOrphanPort(frontendUrl, $"front:{system.Id}");
            }

            if (await IsFrontendReachableAsync(frontendUrl, ct).ConfigureAwait(false))
            {
                return true;
            }

            if (!TryResolveDirectoryPath(system.FrontendProjectPath, out var frontendDir, out var resolveError))
            {
                _logger.LogWarning(
                    "Não foi possível resolver FrontendProjectPath do {Id}: {Error}",
                    system.Id,
                    resolveError);
                return false;
            }

            if (!TryStartFrontend(system, frontendDir, out var startError, out var frontLogPath))
            {
                _logger.LogWarning("Falha ao iniciar frontend {Id}: {Error}", system.Id, startError);
                return false;
            }

            _logger.LogInformation(
                "Frontend {Id} iniciado em {Dir}. Aguardando {Url}… (log: {Log})",
                system.Id,
                frontendDir,
                frontendUrl,
                frontLogPath);

            var frontPid = _launchedFrontPids.TryGetValue(system.Id, out var pid) ? pid : 0;
            var ready = await WaitUntilAsync(
                () => IsFrontendReachableAsync(frontendUrl, ct),
                timeoutSeconds,
                ct,
                elapsed => _logger.LogInformation(
                    "Aguardando Angular {Id} em {Url}… ({Elapsed}s / tipicamente 1–3 min na 1ª vez)",
                    system.Id,
                    frontendUrl,
                    elapsed),
                isAlive: () => frontPid <= 0 || IsProcessAlive(frontPid)).ConfigureAwait(false);

            if (!ready)
            {
                var dead = frontPid > 0 && !IsProcessAlive(frontPid);
                var tail = TryReadLogTail(frontLogPath, 40);
                _logger.LogWarning(
                    "Frontend {Id} não respondeu em {Timeout}s em {Url}.{Dead} Log: {Log}{Tail}",
                    system.Id,
                    timeoutSeconds,
                    frontendUrl,
                    dead ? " Processo npm/node já encerrou." : "",
                    frontLogPath,
                    string.IsNullOrWhiteSpace(tail) ? "" : $"{Environment.NewLine}--- tail ---{Environment.NewLine}{tail}");
            }

            return ready;
        }
        finally
        {
            _frontLocks.TryRemove(system.Id, out _);
        }
    }

    private bool TryStartMonitorApi(
        OrchestratorSystemOptions system,
        string projectPath,
        out string? error)
    {
        error = null;

        try
        {
            var urls = system.ResolveApiBase().Trim().TrimEnd('/');
            var workDir = Path.GetDirectoryName(projectPath)!;
            var sourceBinDir = Path.Combine(workDir, "bin", "Debug", "net8.0");
            var dllName = ResolveApiDllName(projectPath, sourceBinDir);
            var dllPath = Path.Combine(sourceBinDir, dllName);

            ProcessStartInfo psi;
            if (File.Exists(dllPath))
            {
                // Shadow copy: o processo NÃO segura as DLLs do bin do repo —
                // VS/dotnet build no Receptor/Arquivador deixa de falhar com file lock.
                var shadowDir = ShadowCopyMonitorBin(sourceBinDir, system.Id);
                var shadowDll = Path.Combine(shadowDir, dllName);
                _logger.LogDebug(
                    "LocalDev: API {Id} rodando de shadow {Shadow} ({Dll})",
                    system.Id,
                    shadowDir,
                    dllName);
                psi = CreateSilentProcessStartInfo(
                    "dotnet",
                    $"\"{shadowDll}\"",
                    shadowDir);
            }
            else
            {
                // Fallback: dotnet run (compila se preciso).
                _logger.LogWarning(
                    "LocalDev: {Dll} ausente — usando dotnet run. Compile o projeto na 1ª vez.",
                    dllPath);
                psi = CreateSilentProcessStartInfo(
                    "dotnet",
                    $"run --project \"{projectPath}\" --no-launch-profile",
                    workDir);
            }

            psi.Environment["ASPNETCORE_ENVIRONMENT"] = "Development";
            psi.Environment["ASPNETCORE_URLS"] = urls;

            // Shadow %TEMP% quebra a descoberta por BaseDirectory — injeta a raiz do pacote
            // (1-Receptor / 2-Arquivador) para o DevHost achar tools\*.DevHost.exe.
            var packageRoot = TryResolveMonitorPackageRoot(projectPath);
            if (!string.IsNullOrWhiteSpace(packageRoot))
            {
                InjectMonitorPackageRootEnv(psi, system.Id, packageRoot);
                _logger.LogDebug(
                    "LocalDev: {Id} Monitor__*RootPath={PackageRoot}",
                    system.Id,
                    packageRoot);
            }

            var process = StartSilentProcess(psi);
            if (process is null)
            {
                error = "Process.Start retornou null.";
                return false;
            }

            AssignToJob(process);
            _launchedApiPids[system.Id] = process.Id;
            Thread.Sleep(500);
            if (process.HasExited)
            {
                error = $"Processo saiu imediatamente (exit={process.ExitCode}). Compile o Monitor.Api primeiro.";
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            error = ex.Message;
            return false;
        }
    }

    private bool TryStartFrontend(
        OrchestratorSystemOptions system,
        string frontendDir,
        out string? error,
        out string logPath)
    {
        error = null;
        logPath = Path.Combine(Path.GetTempPath(), $"cte-orq-front-{system.Id}.log");

        try
        {
            if (!Directory.Exists(Path.Combine(frontendDir, "node_modules")))
            {
                error =
                    $"node_modules ausente em {frontendDir}. Rode: npm install";
                return false;
            }

            var nxJs = Path.Combine(frontendDir, "node_modules", "nx", "bin", "nx.js");
            if (!File.Exists(nxJs))
            {
                error = $"nx.js ausente em {nxJs}. Rode npm install no Frontend.";
                return false;
            }

            if (!TryResolveNxAppName(frontendDir, out var appName))
            {
                error = $"Nenhum app Nx em {frontendDir}\\apps.";
                return false;
            }

            var port = 0;
            LocalDevPortReclaimer.TryGetPortFromUrl(system.ResolveFrontendUrl(), out port);
            if (port <= 0)
            {
                error = $"FrontendUrl sem porta válida ({system.ResolveFrontendUrl()}).";
                return false;
            }

            try
            {
                File.WriteAllText(
                    logPath,
                    $"=== cte LocalDev nx serve {system.Id} ({appName} :{port}) @ {DateTimeOffset.Now:o} ==={Environment.NewLine}" +
                    $"dir={frontendDir}{Environment.NewLine}" +
                    $"nx={nxJs}{Environment.NewLine}{Environment.NewLine}");
            }
            catch
            {
                // ignore
            }

            // Chama nx.js direto — NÃO usa "npm start" (evita prestart/free-port.cjs ausente na VDI).
            // Porta já foi liberada por TryReclaimOrphanPort.
            var node = ResolveNodeExecutable();
            var args =
                $"/d /c \"\"{node}\" \"{nxJs}\" serve {appName} --port={port} >> \"{logPath}\" 2>&1\"";
            var psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = args,
                WorkingDirectory = frontendDir,
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardOutput = false,
                RedirectStandardError = false,
                RedirectStandardInput = false,
            };
            // Sem prompt "Port already in use?" — falha no log em vez de travar.
            psi.Environment["CI"] = "true";
            psi.Environment["NG_CLI_ANALYTICS"] = "false";

            var process = Process.Start(psi);
            if (process is null)
            {
                error = "Process.Start (frontend) retornou null.";
                return false;
            }

            AssignToJob(process);
            _launchedFrontPids[system.Id] = process.Id;
            Thread.Sleep(1500);
            if (process.HasExited)
            {
                var tail = TryReadLogTail(logPath, 30);
                error =
                    $"nx serve saiu imediatamente (exit={process.ExitCode}). " +
                    $"Veja {logPath}. Rode npm install no Frontend se necessário." +
                    (string.IsNullOrWhiteSpace(tail) ? "" : $"{Environment.NewLine}{tail}");
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            error = ex.Message;
            return false;
        }
    }

    private static string ResolveNodeExecutable()
    {
        // process.execPath equivalente: "node" no PATH do usuário LocalDev.
        return "node";
    }

    private static bool TryResolveNxAppName(string frontendDir, out string appName)
    {
        appName = string.Empty;
        var appsDir = Path.Combine(frontendDir, "apps");
        if (!Directory.Exists(appsDir))
        {
            return false;
        }

        foreach (var dir in Directory.EnumerateDirectories(appsDir).OrderBy(d => d, StringComparer.OrdinalIgnoreCase))
        {
            var name = Path.GetFileName(dir);
            if (name.EndsWith("-e2e", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if (File.Exists(Path.Combine(dir, "project.json")))
            {
                appName = name;
                return true;
            }
        }

        return false;
    }

    /// <summary>Processo sem janela (CreateNoWindow + discard stdout/stderr).</summary>
    private static ProcessStartInfo CreateSilentProcessStartInfo(
        string fileName,
        string arguments,
        string workingDirectory) =>
        new()
        {
            FileName = fileName,
            Arguments = arguments,
            WorkingDirectory = workingDirectory,
            UseShellExecute = false,
            CreateNoWindow = true,
            WindowStyle = ProcessWindowStyle.Hidden,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            RedirectStandardInput = false,
        };

    private static Process? StartSilentProcess(ProcessStartInfo psi)
    {
        var process = Process.Start(psi);
        if (process is null)
        {
            return null;
        }

        process.OutputDataReceived += static (_, _) => { };
        process.ErrorDataReceived += static (_, _) => { };
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        return process;
    }

    /// <summary>
    /// Monitor.Api.dll (cadeia) ou CTe.Resgate.Api.dll (sidecar) — nome do .csproj.
    /// </summary>
    private static string ResolveApiDllName(string projectPath, string sourceBinDir)
    {
        var fromProject = Path.GetFileNameWithoutExtension(projectPath) + ".dll";
        if (File.Exists(Path.Combine(sourceBinDir, fromProject)))
        {
            return fromProject;
        }

        if (File.Exists(Path.Combine(sourceBinDir, "Monitor.Api.dll")))
        {
            return "Monitor.Api.dll";
        }

        return fromProject;
    }

    /// <summary>
    /// ProjectPath: …/{Pacote}/Xxx.Api/src/Monitor.Api/Monitor.Api.csproj → raiz {Pacote}
    /// (1-Receptor, 2-Arquivador, …).
    /// </summary>
    private static string? TryResolveMonitorPackageRoot(string projectPath)
    {
        try
        {
            var monitorApiDir = Path.GetDirectoryName(Path.GetFullPath(projectPath));
            if (string.IsNullOrWhiteSpace(monitorApiDir))
            {
                return null;
            }

            var src = Directory.GetParent(monitorApiDir)?.FullName;
            var apiFolder = src is null ? null : Directory.GetParent(src)?.FullName;
            var packageRoot = apiFolder is null ? null : Directory.GetParent(apiFolder)?.FullName;
            if (string.IsNullOrWhiteSpace(packageRoot) || !Directory.Exists(packageRoot))
            {
                return null;
            }

            return Path.GetFullPath(packageRoot);
        }
        catch
        {
            return null;
        }
    }

    private static void InjectMonitorPackageRootEnv(
        ProcessStartInfo psi,
        string systemId,
        string packageRoot)
    {
        // ASP.NET Core env: Monitor__ReceptorRootPath etc.
        if (systemId.Equals("receptor", StringComparison.OrdinalIgnoreCase))
        {
            psi.Environment["Monitor__ReceptorRootPath"] = packageRoot;
            return;
        }

        if (systemId.Equals("arquivador", StringComparison.OrdinalIgnoreCase))
        {
            psi.Environment["Monitor__ArquivadorRootPath"] = packageRoot;
            return;
        }

        if (systemId.Equals("sintetizador", StringComparison.OrdinalIgnoreCase))
        {
            psi.Environment["Monitor__SintetizadorRootPath"] = packageRoot;
            return;
        }

        if (systemId.Equals("analisador", StringComparison.OrdinalIgnoreCase))
        {
            psi.Environment["Monitor__AnalisadorRootPath"] = packageRoot;
            return;
        }

        if (systemId.Equals("integrador", StringComparison.OrdinalIgnoreCase))
        {
            psi.Environment["Monitor__IntegradorRootPath"] = packageRoot;
            return;
        }

        if (systemId.Equals("carga", StringComparison.OrdinalIgnoreCase))
        {
            psi.Environment["Monitor__CargaRootPath"] = packageRoot;
            return;
        }

        // Fallback genérico para futuros monitores.
        psi.Environment["Monitor__PackageRootPath"] = packageRoot;
    }

    private bool TryResolveFilePath(string configured, out string absolutePath, out string? error)
    {
        var root = ResolveRepoRoot();
        return CtePathResolver.TryResolveFile(configured, root, out absolutePath, out error);
    }

    private bool TryResolveDirectoryPath(string configured, out string absolutePath, out string? error)
    {
        var root = ResolveRepoRoot();
        return CtePathResolver.TryResolveDirectory(configured, root, out absolutePath, out error);
    }

    /// <summary>
    /// Pasta CT_e (contém 0-Orquestrador). Prefixo da máquina é irrelevante —
    /// descoberta sobe a partir do ContentRoot / cwd / BaseDirectory.
    /// </summary>
    private string? ResolveRepoRoot()
    {
        var configured = _options.Value.LocalDev.RepoRoot;
        var root = CtePathResolver.ResolveRepoRoot(configured, _env.ContentRootPath);
        if (root is null && !string.IsNullOrWhiteSpace(configured))
        {
            _logger.LogWarning(
                "LocalDev:RepoRoot inválido ou de outra máquina ({RepoRoot}). Usando descoberta automática a partir de Orquestrador.Api.",
                configured);
        }
        else if (root is not null)
        {
            _logger.LogDebug("LocalDev RepoRoot resolvido: {RepoRoot}", root);
        }

        return root;
    }

    private static async Task<bool> WaitUntilAsync(
        Func<Task<bool>> probe,
        int timeoutSeconds,
        CancellationToken ct,
        Action<int>? onProgressSeconds = null,
        Func<bool>? isAlive = null)
    {
        var timeout = TimeSpan.FromSeconds(Math.Clamp(timeoutSeconds, 5, 300));
        var deadline = DateTimeOffset.UtcNow + timeout;
        var started = DateTimeOffset.UtcNow;
        var lastProgressAt = -1;

        while (DateTimeOffset.UtcNow < deadline)
        {
            ct.ThrowIfCancellationRequested();

            if (isAlive is not null && !isAlive())
            {
                return false;
            }

            if (await probe().ConfigureAwait(false))
            {
                return true;
            }

            var elapsed = (int)(DateTimeOffset.UtcNow - started).TotalSeconds;
            if (onProgressSeconds is not null && elapsed > 0 && elapsed / 15 != lastProgressAt)
            {
                lastProgressAt = elapsed / 15;
                onProgressSeconds(elapsed);
            }

            await Task.Delay(1000, ct).ConfigureAwait(false);
        }

        return false;
    }

    /// <summary>
    /// Probe sem HttpClient quando possível: evita first-chance HttpRequestException
    /// a cada segundo no Output do Visual Studio enquanto Angular sobe.
    /// </summary>
    private async Task<bool> IsHttpOkAsync(string url, CancellationToken ct)
    {
        if (TryGetLocalPort(url, out var port) && !IsLocalTcpPortListening(port))
        {
            return false;
        }

        try
        {
            using var response = await _healthClient.GetAsync(url, ct).ConfigureAwait(false);
            return (int)response.StatusCode < 500;
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or IOException or SocketException)
        {
            return false;
        }
    }

    /// <summary>
    /// Pronto de verdade: index com app-root + script de bundle (não só o shell HTTP do vite/webpack).
    /// </summary>
    private async Task<bool> IsAngularDevServerReadyAsync(string frontendUrl, CancellationToken ct)
    {
        if (TryGetLocalPort(frontendUrl, out var port) && !IsLocalTcpPortListening(port))
        {
            return false;
        }

        try
        {
            using var response = await _healthClient.GetAsync(frontendUrl, ct).ConfigureAwait(false);
            if ((int)response.StatusCode >= 500)
            {
                return false;
            }

            var html = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(html))
            {
                return false;
            }

            // index.html mínimo do Angular + pelo menos um script (dev-server injeta após 1ª compile).
            var hasRoot = html.Contains("app-root", StringComparison.OrdinalIgnoreCase);
            var hasScript =
                html.Contains("<script", StringComparison.OrdinalIgnoreCase) ||
                html.Contains("main.js", StringComparison.OrdinalIgnoreCase) ||
                html.Contains("polyfills", StringComparison.OrdinalIgnoreCase);
            return hasRoot && hasScript;
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or IOException or SocketException)
        {
            return false;
        }
    }

    private static bool TryGetLocalPort(string url, out int port)
    {
        port = 0;
        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
        {
            return false;
        }

        if (!string.Equals(uri.Host, "localhost", StringComparison.OrdinalIgnoreCase) &&
            uri.Host is not "127.0.0.1" and not "::1")
        {
            return false;
        }

        port = uri.IsDefaultPort
            ? (string.Equals(uri.Scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase) ? 443 : 80)
            : uri.Port;
        return port > 0;
    }

    private static bool IsLocalTcpPortListening(int port)
    {
        try
        {
            var listeners = IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners();
            return listeners.Any(ep => ep.Port == port);
        }
        catch
        {
            return false;
        }
    }

    private static bool IsProcessAlive(int pid)
    {
        try
        {
            var p = Process.GetProcessById(pid);
            return !p.HasExited;
        }
        catch
        {
            return false;
        }
    }

    private static string TryReadLogTail(string path, int maxLines)
    {
        try
        {
            if (!File.Exists(path))
            {
                return string.Empty;
            }

            var lines = File.ReadAllLines(path);
            if (lines.Length == 0)
            {
                return string.Empty;
            }

            var start = Math.Max(0, lines.Length - maxLines);
            return string.Join(Environment.NewLine, lines.Skip(start));
        }
        catch
        {
            return string.Empty;
        }
    }

    /// <inheritdoc />
    public Task<(bool Success, string Message)> EnsureDevHostsBuiltAsync(CancellationToken ct)
    {
        return Task.Run(() =>
        {
            ct.ThrowIfCancellationRequested();
            var root = ResolveRepoRoot();
            if (root is null)
            {
                return (false, "Raiz CT_e não encontrada — não foi possível compilar DevHosts.");
            }

            _logger.LogInformation("LocalDev: garantindo DevHosts (Debug) em {RepoRoot}…", root);
            var (built, skipped, errors) = DevHostMsBuild.EnsureAll(root);

            if (errors.Count > 0)
            {
                var detail = string.Join("; ", errors.Take(4));
                _logger.LogWarning(
                    "LocalDev: DevHosts incompletos (built={Built} skip={Skipped} err={ErrCount}): {Detail}",
                    built,
                    skipped,
                    errors.Count,
                    detail);
                return (
                    false,
                    $"DevHosts: {built} compilado(s), {skipped} ok, {errors.Count} falha(s). {detail}");
            }

            var msg = built == 0
                ? $"DevHosts já presentes ({skipped})."
                : $"DevHosts: {built} compilado(s), {skipped} já existia(m).";
            _logger.LogInformation("LocalDev: {Msg}", msg);
            return (true, msg);
        }, ct);
    }

    /// <inheritdoc />
    public void KillLaunchedChildren()
    {
        foreach (var kv in _launchedFrontPids.ToArray())
        {
            TryKillProcessTree(kv.Value, $"front:{kv.Key}");
            _launchedFrontPids.TryRemove(kv.Key, out _);
        }

        foreach (var kv in _launchedApiPids.ToArray())
        {
            TryKillProcessTree(kv.Value, $"api:{kv.Key}");
            _launchedApiPids.TryRemove(kv.Key, out _);
        }

        // Fecha o Job Object → KILL_ON_JOB_CLOSE mata qualquer filho restante
        // (inclui netinhos npm→node que o taskkill possa ter perdido).
        lock (_jobLock)
        {
            _job?.Dispose();
            _job = null;
        }
    }

    private void TryReclaimOrphanPort(string? url, string label)
    {
        if (!LocalDevPortReclaimer.TryGetPortFromUrl(url, out var port))
        {
            return;
        }

        // Sempre garante a porta livre (IPv4/IPv6/netstat) antes do spawn —
        // evita o prompt interativo do Angular "Port already in use".
        if (!LocalDevPortReclaimer.TryEnsurePortFree(port, out var detail))
        {
            _logger.LogWarning(
                "LocalDev: não liberou porta {Port} ({Label}): {Detail}",
                port,
                label,
                detail);
            return;
        }

        if (!string.IsNullOrWhiteSpace(detail) &&
            detail.Contains("encerrado", StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogWarning(
                "LocalDev: porta {Port} liberada ({Label}): {Detail}",
                port,
                label,
                detail);
        }
    }

    private void AssignToJob(Process process)
    {
        try
        {
            GetOrCreateJob().Assign(process);
        }
        catch (Exception ex)
        {
            // Sem job, Shift+F5 ainda pode deixar órfãos — KillLaunchedChildren + reclaim cobrem.
            _logger.LogWarning(
                ex,
                "LocalDev: não foi possível anexar pid {Pid} ao Job Object (KILL_ON_JOB_CLOSE). " +
                "Órfãos serão limpos no próximo boot via reclaim de porta.",
                process.Id);
        }
    }

    private LocalDevJobObject GetOrCreateJob()
    {
        lock (_jobLock)
        {
            _job ??= new LocalDevJobObject();
            return _job;
        }
    }

    /// <summary>
    /// Copia bin\Debug\net8.0 para %TEMP%\cte-orq-monitor\{id} e retorna o destino.
    /// O processo .NET Host segura só a cópia — rebuild no repo não conflita.
    /// </summary>
    private static string ShadowCopyMonitorBin(string sourceBinDir, string systemId)
    {
        var destRoot = Path.Combine(Path.GetTempPath(), "cte-orq-monitor", SanitizeId(systemId));
        try
        {
            if (Directory.Exists(destRoot))
            {
                Directory.Delete(destRoot, recursive: true);
            }
        }
        catch
        {
            destRoot = Path.Combine(
                Path.GetTempPath(),
                "cte-orq-monitor",
                $"{SanitizeId(systemId)}-{Guid.NewGuid():N}"[..24]);
        }

        CopyDirectory(sourceBinDir, destRoot);
        return destRoot;
    }

    private static string SanitizeId(string id)
    {
        foreach (var c in Path.GetInvalidFileNameChars())
        {
            id = id.Replace(c, '_');
        }

        return string.IsNullOrWhiteSpace(id) ? "monitor" : id;
    }

    private static void CopyDirectory(string sourceDir, string destDir)
    {
        Directory.CreateDirectory(destDir);
        foreach (var file in Directory.GetFiles(sourceDir))
        {
            var name = Path.GetFileName(file);
            // PDBs grandes — opcional; manter para stack traces úteis no Output.
            File.Copy(file, Path.Combine(destDir, name), overwrite: true);
        }

        foreach (var dir in Directory.GetDirectories(sourceDir))
        {
            CopyDirectory(dir, Path.Combine(destDir, Path.GetFileName(dir)));
        }
    }

    private void TryKillProcessTree(int pid, string label)
    {
        if (pid <= 0)
        {
            return;
        }

        try
        {
            // /T mata a árvore (dotnet/npm → node filhos) e libera DLLs.
            using var killer = Process.Start(new ProcessStartInfo
            {
                FileName = "taskkill",
                Arguments = $"/PID {pid} /T /F",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            });
            killer?.WaitForExit(5000);
            _logger.LogInformation("LocalDev: encerrado processo {Label} (pid {Pid})", label, pid);
        }
        catch (Exception ex)
        {
            _logger.LogDebug(ex, "LocalDev: falha ao encerrar {Label} pid {Pid}", label, pid);
        }
    }

    private static string Combine(string baseUrl, string path) =>
        $"{baseUrl.TrimEnd('/')}/{path.TrimStart('/')}";

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        KillLaunchedChildren();
        _healthClient.Dispose();
    }
}
