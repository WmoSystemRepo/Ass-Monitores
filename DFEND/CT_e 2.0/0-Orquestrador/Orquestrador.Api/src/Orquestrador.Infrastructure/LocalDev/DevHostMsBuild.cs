using System.Diagnostics;
using System.Text;

namespace Orquestrador.Infrastructure.LocalDev;

/// <summary>
/// Compila tools/*.DevHost (Debug net47) via MSBuild — usado no boot e no Ligar cadeia
/// para que Sintetizador/Analisador/etc. tenham o .exe antes do POST /service/start.
/// </summary>
internal static class DevHostMsBuild
{
    private static readonly (string RelCsproj, string RelExe)[] KnownHosts =
    [
        (
            @"1-Receptor\tools\Receptor.DevHost\Receptor.DevHost.csproj",
            @"1-Receptor\tools\Receptor.DevHost\bin\Debug\Receptor.DevHost.exe"),
        (
            @"2-Arquivador\tools\Arquivador.DevHost\Arquivador.DevHost.csproj",
            @"2-Arquivador\tools\Arquivador.DevHost\bin\Debug\Arquivador.DevHost.exe"),
        (
            @"3-Sintetizador\tools\Sintetizador.DevHost\Sintetizador.DevHost.csproj",
            @"3-Sintetizador\tools\Sintetizador.DevHost\bin\Debug\Sintetizador.DevHost.exe"),
        (
            @"4-Analisador\tools\Analisador.DevHost\Analisador.DevHost.csproj",
            @"4-Analisador\tools\Analisador.DevHost\bin\Debug\Analisador.DevHost.exe"),
        (
            @"5-Integrador\tools\Integrador.DevHost\Integrador.DevHost.csproj",
            @"5-Integrador\tools\Integrador.DevHost\bin\Debug\Integrador.DevHost.exe"),
        (
            @"6-Carga\tools\Carga.DevHost\Carga.DevHost.csproj",
            @"6-Carga\tools\Carga.DevHost\bin\Debug\Carga.DevHost.exe"),
    ];

    public static (int Built, int Skipped, IReadOnlyList<string> Errors) EnsureAll(string repoRoot)
    {
        var errors = new List<string>();
        var built = 0;
        var skipped = 0;

        foreach (var (relCsproj, relExe) in KnownHosts)
        {
            var csproj = Path.Combine(repoRoot, relCsproj);
            var exe = Path.Combine(repoRoot, relExe);

            if (!File.Exists(csproj))
            {
                errors.Add($"Projeto ausente: {relCsproj}");
                continue;
            }

            if (File.Exists(exe))
            {
                skipped++;
                continue;
            }

            if (!TryBuild(csproj, exe, out var error))
            {
                errors.Add($"{relCsproj}: {error}");
                continue;
            }

            built++;
        }

        return (built, skipped, errors);
    }

    public static bool TryBuild(string csprojPath, string expectedExePath, out string? error)
    {
        error = null;
        if (File.Exists(expectedExePath))
            return true;

        if (!File.Exists(csprojPath))
        {
            error = $"Projeto DevHost não encontrado: {csprojPath}";
            return false;
        }

        var tool = FindBuildTool();
        if (tool is null)
        {
            error =
                "MSBuild não encontrado. Instale Visual Studio (.NET desktop) ou rode: " +
                @"0-Orquestrador\tools\build-devhosts.ps1";
            return false;
        }

        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = tool.FileName,
                Arguments = tool.ArgumentsPrefix + $"\"{csprojPath}\" /p:Configuration=Debug /v:m /nologo",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8
            };

            using var p = Process.Start(psi);
            if (p is null)
            {
                error = "Falha ao iniciar MSBuild.";
                return false;
            }

            var stdoutTask = p.StandardOutput.ReadToEndAsync();
            var stderrTask = p.StandardError.ReadToEndAsync();
            if (!p.WaitForExit(300_000))
            {
                try { p.Kill(entireProcessTree: true); } catch { /* ignore */ }
                error = "MSBuild excedeu 5 minutos.";
                return false;
            }

            var stdout = stdoutTask.GetAwaiter().GetResult();
            var stderr = stderrTask.GetAwaiter().GetResult();

            if (p.ExitCode != 0 || !File.Exists(expectedExePath))
            {
                var detail = string.Join(
                    " | ",
                    new[] { stderr?.Trim(), Truncate(stdout?.Trim(), 800) }
                        .Where(s => !string.IsNullOrWhiteSpace(s)));
                error =
                    $"Falha ao compilar DevHost (exit={p.ExitCode}). " +
                    $"Rode 0-Orquestrador\\tools\\build-devhosts.ps1. {detail}";
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            error = $"Falha MSBuild: {ex.Message}";
            return false;
        }
    }

    private sealed record BuildTool(string FileName, string ArgumentsPrefix);

    private static BuildTool? FindBuildTool()
    {
        var vswhere = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
            "Microsoft Visual Studio", "Installer", "vswhere.exe");

        if (File.Exists(vswhere))
        {
            try
            {
                using var p = Process.Start(new ProcessStartInfo
                {
                    FileName = vswhere,
                    Arguments = "-latest -requires Microsoft.Component.MSBuild -find MSBuild\\**\\Bin\\MSBuild.exe",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                });
                var line = p?.StandardOutput.ReadLine()?.Trim();
                p?.WaitForExit(15_000);
                if (!string.IsNullOrWhiteSpace(line) && File.Exists(line))
                    return new BuildTool(line, string.Empty);
            }
            catch
            {
                // ignore
            }
        }

        foreach (var candidate in new[]
                 {
                     @"C:\Program Files\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\MSBuild.exe",
                     @"C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe",
                     @"C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe",
                     @"C:\Program Files\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\MSBuild.exe"
                 })
        {
            if (File.Exists(candidate))
                return new BuildTool(candidate, string.Empty);
        }

        // Fallback fraco — projetos net47 costumam precisar do MSBuild do VS.
        return new BuildTool("dotnet", "msbuild ");
    }

    private static string? Truncate(string? text, int max)
    {
        if (string.IsNullOrEmpty(text) || text.Length <= max)
            return text;
        return text[..max] + "…";
    }
}
