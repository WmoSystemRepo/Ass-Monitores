namespace CTe.Resgate.Domain;

public static class PassoResgate
{
    public const string P0 = "P0";
    public const string P1 = "P1";
    public const string P2 = "P2";
    public const string P3 = "P3";
    public const string P4 = "P4";
    public const string P5a = "P5a";
    public const string P5b = "P5b";
    public const string P5c = "P5c";
    public const string P5d = "P5d";
    public const string P6 = "P6";
    public const string P7 = "P7";
}

public static class ItemStatus
{
    public const string Pendente = "Pendente";
    public const string EmProcessamento = "EmProcessamento";
    public const string Recuperado = "Recuperado";
    public const string Existente = "Existente";
    public const string NaoLocalizado = "NaoLocalizado";
    public const string Erro = "Erro";
}

public static class LoteStatus
{
    public const string Aberto = "Aberto";
    public const string Processando = "Processando";
    public const string Concluido = "Concluido";
}

public static class ChaveAccessRules
{
    public const int MinCount = 1;
    public const int MaxCount = 1000;
    public const int KeyLength = 44;
    public const long MaxUploadBytes = 5 * 1024 * 1024;

    public static bool IsValidKey(string? chave)
        => !string.IsNullOrWhiteSpace(chave)
           && chave.Length == KeyLength
           && chave.All(char.IsDigit);

    public static string Mask(string chave)
        => chave.Length != KeyLength
            ? "****"
            : $"{chave[..6]}****{chave[^6..]}";

    /// <summary>Normaliza lista: trim, remove vazios, dedupe preservando ordem, valida.</summary>
    public static (IReadOnlyList<string> Keys, IReadOnlyList<string> Errors) Normalize(IEnumerable<string> raw)
    {
        var errors = new List<string>();
        var seen = new HashSet<string>(StringComparer.Ordinal);
        var keys = new List<string>();
        var index = 0;
        foreach (var line in raw)
        {
            index++;
            var k = (line ?? string.Empty).Trim();
            if (k.Length == 0) continue;
            if (!IsValidKey(k))
            {
                errors.Add($"Linha {index}: chave inválida (exige 44 dígitos).");
                continue;
            }
            if (seen.Add(k))
                keys.Add(k);
        }

        if (keys.Count < MinCount)
            errors.Add($"É necessário no mínimo {MinCount} chave válida.");
        if (keys.Count > MaxCount)
            errors.Add($"Máximo de {MaxCount} chaves por lote (obtido {keys.Count}).");

        // Spec: rejeitar lote se houver inválida
        if (errors.Any(e => e.Contains("inválida", StringComparison.OrdinalIgnoreCase)))
            return (Array.Empty<string>(), errors);

        if (keys.Count < MinCount || keys.Count > MaxCount)
            return (Array.Empty<string>(), errors);

        return (keys, Array.Empty<string>());
    }
}
