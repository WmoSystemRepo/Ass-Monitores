using System.Diagnostics;

namespace Orquestrador.Infrastructure.LocalDev;

/// <summary>Abre URLs no browser padrão (Windows LocalDev).</summary>
internal static class LocalDevBrowser
{
    public static bool TryOpen(string url, out string? error)
    {
        error = null;
        if (string.IsNullOrWhiteSpace(url))
        {
            error = "URL vazia.";
            return false;
        }

        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = url.Trim(),
                UseShellExecute = true
            });
            return true;
        }
        catch (Exception ex)
        {
            error = ex.Message;
            return false;
        }
    }
}
