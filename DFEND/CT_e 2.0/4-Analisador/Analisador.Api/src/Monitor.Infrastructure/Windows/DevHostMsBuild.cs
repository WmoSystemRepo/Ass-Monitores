using System.Diagnostics;

namespace Monitor.Infrastructure.Windows;

/// <summary>
/// Compila tools/*.DevHost (Debug) se o .exe ainda não existir — LocalDev / Ligar cadeia.
/// </summary>
internal static class DevHostMsBuild
{
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

        var msbuild = FindMsBuild();
        if (msbuild is null)
        {
            error =
                "MSBuild não encontrado. Instale Visual Studio (.NET desktop) ou rode: " +
                @"0-Orquestrador\tools\build-devhosts.ps1";
            return false;
        }

        try
        {
            using var p = Process.Start(new ProcessStartInfo
            {
                FileName = msbuild,
                Arguments = $"\"{csprojPath}\" /p:Configuration=Debug /v:q /nologo",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            });

            if (p is null)
            {
                error = "Falha ao iniciar MSBuild.";
                return false;
            }

            p.WaitForExit(300_000);
            if (p.ExitCode != 0 || !File.Exists(expectedExePath))
            {
                var stderr = p.StandardError.ReadToEnd();
                error =
                    $"Falha ao compilar DevHost (exit={p.ExitCode}). " +
                    $"Abra {csprojPath} no VS (Debug) ou rode build-devhosts.ps1. {stderr}";
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

    private static string? FindMsBuild()
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
                    return line;
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
                     @"C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe"
                 })
        {
            if (File.Exists(candidate))
                return candidate;
        }

        return null;
    }
}
