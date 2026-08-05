using System.Text;
using Microsoft.Extensions.Options;
using Monitor.Application.Abstractions;
using Monitor.Application.Services;
using Monitor.Domain.Models;

namespace Monitor.Infrastructure.Live;

/// <summary>
/// Lê monitor-live.log gerado pelo Integrador.DevHost (Debug.WriteLine do Integrador).
/// Arquivo fica ao lado do exe do host POC.
/// </summary>
public sealed class LiveTraceReader : ILiveTraceReader
{
    private readonly MonitorOptions _options;

    public LiveTraceReader(IOptions<MonitorOptions> options)
    {
        _options = options.Value;
    }

    public IReadOnlyList<LiveTraceLine> ReadRecent(int take = 80)
    {
        var path = ResolveLiveLogPath();
        if (path is null || !File.Exists(path))
        {
            return Array.Empty<LiveTraceLine>();
        }

        try
        {
            using var fs = new FileStream(
                path,
                FileMode.Open,
                FileAccess.Read,
                FileShare.ReadWrite | FileShare.Delete);
            using var reader = new StreamReader(fs, Encoding.UTF8);
            var text = reader.ReadToEnd();
            if (string.IsNullOrWhiteSpace(text))
            {
                return Array.Empty<LiveTraceLine>();
            }

            var lines = text
                .Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries)
                .Where(l => !IsNoise(l))
                .TakeLast(Math.Max(10, take))
                .Select(ParseLine)
                .Where(x => x is not null)
                .Cast<LiveTraceLine>()
                .ToList();

            return lines;
        }
        catch
        {
            return Array.Empty<LiveTraceLine>();
        }
    }

    private string? ResolveLiveLogPath()
    {
        if (!string.IsNullOrWhiteSpace(_options.LiveTracePath)
            && File.Exists(_options.LiveTracePath))
        {
            return Path.GetFullPath(_options.LiveTracePath);
        }

        var root = FindIntegradorRoot();
        if (root is null)
        {
            return null;
        }

        var relative = string.IsNullOrWhiteSpace(_options.IntegradorExeRelativePath)
            ? @"tools\Integrador.DevHost\bin\Debug\Integrador.DevHost.exe"
            : _options.IntegradorExeRelativePath;

        var exeDir = Path.GetDirectoryName(Path.Combine(root, relative));
        if (string.IsNullOrWhiteSpace(exeDir))
        {
            return null;
        }

        return Path.Combine(exeDir, "monitor-live.log");
    }

    private string? FindIntegradorRoot()
    {
        if (!string.IsNullOrWhiteSpace(_options.IntegradorRootPath)
            && Directory.Exists(_options.IntegradorRootPath))
        {
            return Path.GetFullPath(_options.IntegradorRootPath);
        }

        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null)
        {
            var markerWs = Path.Combine(dir.FullName, "dfend-cte-integrador-windowsservices");
            var markerApi = Path.Combine(dir.FullName, "Integrador.Api");
            if (Directory.Exists(markerWs) && Directory.Exists(markerApi))
            {
                return dir.FullName;
            }

            // Também aceita pasta aninhada 2-Integrador/ como raiz
            var nested = Path.Combine(dir.FullName, "2-Integrador");
            if (Directory.Exists(Path.Combine(nested, "dfend-cte-integrador-windowsservices"))
                && Directory.Exists(Path.Combine(nested, "Integrador.Api")))
            {
                return nested;
            }

            dir = dir.Parent;
        }

        return null;
    }

    private static bool IsNoise(string line)
    {
        var t = line.Trim();
        if (t.Length == 0) return true;
        // Cabeçalhos vazios / lixo do TraceListener
        if (t.StartsWith("---", StringComparison.Ordinal)) return true;
        return false;
    }

    private static LiveTraceLine? ParseLine(string raw)
    {
        var text = raw.Trim();
        if (text.Length == 0) return null;

        DateTimeOffset? at = null;
        // [2026-07-22 14:31:00.123] [BOOTSTRAP] msg
        var m = System.Text.RegularExpressions.Regex.Match(
            text,
            @"^\[(?<ts>\d{4}-\d{2}-\d{2}[ T]\d{2}:\d{2}:\d{2}(?:\.\d+)?)\]\s*(?:\[(?<step>[^\]]+)\])?\s*(?<msg>.*)$");
        if (m.Success)
        {
            if (DateTime.TryParse(m.Groups["ts"].Value, out var dt))
            {
                at = new DateTimeOffset(DateTime.SpecifyKind(dt, DateTimeKind.Local));
            }

            var step = m.Groups["step"].Success ? m.Groups["step"].Value : null;
            var msg = m.Groups["msg"].Value.Trim();
            if (string.IsNullOrWhiteSpace(msg) && !string.IsNullOrWhiteSpace(step))
            {
                msg = step;
            }

            return new LiveTraceLine(at ?? DateTimeOffset.Now, msg, step, "debug");
        }

        // Linha crua do Integrador: DFEND_CTe_Integrador conexao iniciando...
        return new LiveTraceLine(DateTimeOffset.Now, text, null, "debug");
    }
}
