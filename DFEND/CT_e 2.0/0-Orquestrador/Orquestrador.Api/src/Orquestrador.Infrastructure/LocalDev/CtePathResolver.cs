namespace Orquestrador.Infrastructure.LocalDev;

/// <summary>
/// Resolve caminhos do monorepo CT_e sem depender do prefixo absoluto da máquina
/// (C:\Users\..., D:\Clones\..., etc.). A âncora estável é a pasta CT_e que contém
/// 0-Orquestrador / 1-Receptor / …
/// </summary>
internal static class CtePathResolver
{
    /// <summary>Pastas de primeiro nível do monorepo (marcadores estáveis).</summary>
    private static readonly string[] RepoMarkers =
    [
        "0-Orquestrador",
        "1-Receptor",
        "2-Arquivador",
    ];

    /// <summary>
    /// Descobre a raiz CT_e. <paramref name="configuredRepoRoot"/> só é usado se existir;
    /// caminhos absolutos de outra máquina são ignorados e a descoberta automática prevalece.
    /// </summary>
    public static string? ResolveRepoRoot(
        string? configuredRepoRoot,
        params string?[] searchRoots)
    {
        if (!string.IsNullOrWhiteSpace(configuredRepoRoot))
        {
            try
            {
                var configured = Path.GetFullPath(configuredRepoRoot.Trim());
                if (Directory.Exists(configured) && LooksLikeRepoRoot(configured))
                {
                    return configured;
                }
            }
            catch
            {
                // Config inválida — cai na descoberta automática.
            }
        }

        foreach (var start in EnumerateSearchStarts(searchRoots))
        {
            var found = WalkUpForRepoRoot(start);
            if (found is not null)
            {
                return found;
            }
        }

        return null;
    }

    /// <summary>
    /// Resolve arquivo relativo à raiz CT_e. Se vier absoluto de outra máquina
    /// (ex.: C:\Users\outro\...\CT_e\1-Receptor\...), remapeia pelo sufixo estável.
    /// </summary>
    public static bool TryResolveFile(
        string configured,
        string? repoRoot,
        out string absolutePath,
        out string? error)
    {
        absolutePath = string.Empty;
        error = null;

        var trimmed = configured.Trim();
        if (string.IsNullOrWhiteSpace(trimmed))
        {
            error = "Caminho vazio.";
            return false;
        }

        if (Path.IsPathRooted(trimmed) && File.Exists(trimmed))
        {
            absolutePath = Path.GetFullPath(trimmed);
            return true;
        }

        if (repoRoot is null)
        {
            error = "Raiz CT_e não encontrada (subindo a partir de Orquestrador.Api).";
            return false;
        }

        var relative = ToRepoRelative(trimmed);
        var candidate = Path.GetFullPath(Path.Combine(repoRoot, relative));
        if (File.Exists(candidate))
        {
            absolutePath = candidate;
            return true;
        }

        error = $"Arquivo não encontrado: '{candidate}' (RepoRoot={repoRoot}; original='{trimmed}').";
        return false;
    }

    /// <summary>
    /// Resolve pasta relativa à raiz CT_e, com o mesmo remapeamento de paths absolutos voláteis.
    /// </summary>
    public static bool TryResolveDirectory(
        string configured,
        string? repoRoot,
        out string absolutePath,
        out string? error)
    {
        absolutePath = string.Empty;
        error = null;

        var trimmed = configured.Trim();
        if (string.IsNullOrWhiteSpace(trimmed))
        {
            error = "Caminho vazio.";
            return false;
        }

        if (Path.IsPathRooted(trimmed) && Directory.Exists(trimmed))
        {
            absolutePath = Path.GetFullPath(trimmed);
            return true;
        }

        if (repoRoot is null)
        {
            error = "Raiz CT_e não encontrada (subindo a partir de Orquestrador.Api).";
            return false;
        }

        var relative = ToRepoRelative(trimmed);
        var candidate = Path.GetFullPath(Path.Combine(repoRoot, relative));
        if (Directory.Exists(candidate))
        {
            absolutePath = candidate;
            return true;
        }

        error = $"Pasta não encontrada: '{candidate}' (RepoRoot={repoRoot}; original='{trimmed}').";
        return false;
    }

    /// <summary>
    /// Extrai o sufixo estável a partir de CT_e ou de 0-Orquestrador / 1-Receptor / 2-Arquivador.
    /// Paths já relativos são normalizados.
    /// </summary>
    public static string ToRepoRelative(string path)
    {
        var normalized = path.Replace('/', Path.DirectorySeparatorChar)
            .Replace('\\', Path.DirectorySeparatorChar)
            .Trim();

        // ...\CT_e\1-Receptor\... → 1-Receptor\...
        var cteIdx = IndexOfPathSegment(normalized, "CT_e");
        if (cteIdx >= 0)
        {
            var afterCte = normalized[(cteIdx + "CT_e".Length)..].TrimStart(Path.DirectorySeparatorChar);
            if (afterCte.Length > 0)
            {
                return afterCte;
            }
        }

        // ...\1-Receptor\... ou 1-Receptor\... → a partir do marcador
        foreach (var marker in RepoMarkers)
        {
            var markerIdx = IndexOfPathSegment(normalized, marker);
            if (markerIdx >= 0)
            {
                return normalized[markerIdx..].TrimStart(Path.DirectorySeparatorChar);
            }
        }

        while (normalized.StartsWith($"..{Path.DirectorySeparatorChar}", StringComparison.Ordinal))
        {
            normalized = normalized[3..];
        }

        return normalized.TrimStart(Path.DirectorySeparatorChar);
    }

    private static IEnumerable<string> EnumerateSearchStarts(IEnumerable<string?> searchRoots)
    {
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var results = new List<string>();

        void Add(string? root)
        {
            if (string.IsNullOrWhiteSpace(root))
            {
                return;
            }

            try
            {
                var full = Path.GetFullPath(root);
                if (seen.Add(full))
                {
                    results.Add(full);
                }
            }
            catch
            {
                // ignore
            }
        }

        foreach (var root in searchRoots)
        {
            Add(root);
        }

        try
        {
            Add(Directory.GetCurrentDirectory());
        }
        catch
        {
            // ignore
        }

        try
        {
            Add(AppContext.BaseDirectory);
        }
        catch
        {
            // ignore
        }

        return results;
    }

    private static string? WalkUpForRepoRoot(string start)
    {
        DirectoryInfo? dir;
        try
        {
            dir = Directory.Exists(start)
                ? new DirectoryInfo(start)
                : File.Exists(start)
                    ? new FileInfo(start).Directory
                    : new DirectoryInfo(start);
        }
        catch
        {
            return null;
        }

        while (dir is not null)
        {
            if (LooksLikeRepoRoot(dir.FullName))
            {
                return dir.FullName;
            }

            // ContentRoot tipicamente em ...\CT_e\0-Orquestrador\Orquestrador.Api\src\Orquestrador.Api
            if (string.Equals(dir.Name, "0-Orquestrador", StringComparison.OrdinalIgnoreCase)
                && dir.Parent is not null
                && LooksLikeRepoRoot(dir.Parent.FullName))
            {
                return dir.Parent.FullName;
            }

            dir = dir.Parent;
        }

        return null;
    }

    private static bool LooksLikeRepoRoot(string path) =>
        Directory.Exists(Path.Combine(path, "0-Orquestrador"));

    /// <summary>Índice do início do segmento de pasta no path (case-insensitive).</summary>
    private static int IndexOfPathSegment(string path, string segment)
    {
        var parts = path.Split([Path.DirectorySeparatorChar, '/'], StringSplitOptions.None);
        var offset = 0;
        for (var i = 0; i < parts.Length; i++)
        {
            if (string.Equals(parts[i], segment, StringComparison.OrdinalIgnoreCase))
            {
                return offset;
            }

            offset += parts[i].Length;
            if (i < parts.Length - 1)
            {
                offset += 1; // separator
            }
        }

        return -1;
    }
}
