using System.Diagnostics;
using System.Text;

namespace CTe.Modules.Monitors.WindowsControl;

/// <summary>
/// Compila tools/*.DevHost (Debug) se o .exe ainda não existir — cópia adaptada de
/// 1-Receptor/Receptor.Api/src/Monitor.Infrastructure/Windows/DevHostMsBuild.cs (mesma técnica,
/// namespace único CTe.Modules.Monitors.WindowsControl para uso pelos 6 monitores in-process).
/// </summary>
public static class DevHostMsBuild
{
    public static bool TryBuild(string csprojPath, string expectedExePath, out string? error)
    {
        error = null;
        if (File.Exists(expectedExePath))
        {
            return true;
        }

        if (!File.Exists(csprojPath))
        {
            var relative = csprojPath;
            var orq = relative.IndexOf("0-Orquestrador", StringComparison.OrdinalIgnoreCase);
            if (orq >= 0)
            {
                relative = relative[(orq + "0-Orquestrador".Length)..].TrimStart('\\', '/');
            }

            error =
                $"Projeto DevHost não encontrado: {relative}. " +
                @"Rode tools\build-devhosts.ps1 na pasta 0-Orquestrador deste clone.";
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
                {
                    return new BuildTool(line, string.Empty);
                }
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
            {
                return new BuildTool(candidate, string.Empty);
            }
        }

        return new BuildTool("dotnet", "msbuild ");
    }

    private static string? Truncate(string? text, int max)
    {
        if (string.IsNullOrEmpty(text) || text.Length <= max)
        {
            return text;
        }

        return text[..max] + "…";
    }
}
