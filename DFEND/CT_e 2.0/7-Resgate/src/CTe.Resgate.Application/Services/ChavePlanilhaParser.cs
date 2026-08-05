using System.Text;
using ExcelDataReader;

namespace CTe.Resgate.Application.Services;

/// <summary>Extrai chaves brutas de CSV, TXT ou XLSX (primeira coluna).</summary>
public static class ChavePlanilhaParser
{
    private static readonly HashSet<string> HeaderTokens = new(StringComparer.OrdinalIgnoreCase)
    {
        "chave", "chave_acesso", "chaveacesso"
    };

    public static IEnumerable<string> Parse(Stream stream, string extension)
    {
        var ext = extension.ToLowerInvariant();
        return ext switch
        {
            ".csv" or ".txt" => ParseText(stream),
            ".xlsx" => ParseXlsx(stream),
            _ => throw new ArgumentException($"Extensão não suportada: {extension}", nameof(extension))
        };
    }

    private static IEnumerable<string> ParseText(Stream stream)
    {
        using var reader = new StreamReader(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true);
        var text = reader.ReadToEnd();
        foreach (var line in text.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries))
        {
            var cell = line.Split(';', ',')[0].Trim();
            if (cell.Length == 0 || HeaderTokens.Contains(cell))
                continue;
            yield return cell;
        }
    }

    private static IEnumerable<string> ParseXlsx(Stream stream)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        using var reader = ExcelReaderFactory.CreateReader(stream);
        var isFirstRow = true;
        while (reader.Read())
        {
            var raw = reader.GetValue(0)?.ToString()?.Trim();
            if (string.IsNullOrWhiteSpace(raw))
                continue;

            if (isFirstRow && HeaderTokens.Contains(raw))
            {
                isFirstRow = false;
                continue;
            }

            isFirstRow = false;
            yield return raw;
        }
    }
}
