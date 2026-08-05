namespace Orquestrador.Infrastructure.LocalDev;

/// <summary>
/// Resolve caminhos do monorepo CT_e sem depender do prefixo absoluto da máquina
/// (C:\Users\..., D:\Clones\..., CT_e / CT_e 2.0 / CT_e 3.0).
/// Âncora preferida: pasta <c>0-Orquestrador</c> (engines + Orquestrador.Api).
/// </summary>
internal static class CtePathResolver
{
    /// <summary>Marcadores estáveis no path (ordem de preferência ao extrair sufixo relativo).</summary>
    private static readonly string[] RepoMarkers =
    [
        "0-Orquestrador",
        "engines",
        "1-Receptor",
        "2-Arquivador",
        "3-Sintetizador",
        "4-Analisador",
        "5-Integrador",
        "6-Carga",
    ];

    /// <summary>
    /// Descobre a raiz de trabalho. <paramref name="configuredRepoRoot"/> só é usado se existir;
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
                    return NormalizeRoot(configured);
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
    /// Resolve arquivo relativo à raiz. Se vier absoluto de outra máquina, remapeia pelo sufixo estável.
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
            error = "Raiz do Orquestrador não encontrada (subindo a partir de Orquestrador.Api).";
            return false;
        }

        foreach (var candidate in EnumerateRelativeCandidates(repoRoot, trimmed))
        {
            if (File.Exists(candidate))
            {
                absolutePath = candidate;
                return true;
            }
        }

        error = $"Arquivo não encontrado sob '{repoRoot}' (original='{trimmed}').";
        return false;
    }

    /// <summary>
    /// Resolve pasta relativa à raiz, com o mesmo remapeamento de paths absolutos voláteis.
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
            error = "Raiz do Orquestrador não encontrada (subindo a partir de Orquestrador.Api).";
            return false;
        }

        foreach (var candidate in EnumerateRelativeCandidates(repoRoot, trimmed))
        {
            if (Directory.Exists(candidate))
            {
                absolutePath = candidate;
                return true;
            }
        }

        error = $"Pasta não encontrada sob '{repoRoot}' (original='{trimmed}').";
        return false;
    }

    /// <summary>
    /// Extrai o sufixo estável a partir de CT_e* / 0-Orquestrador / engines / 1-Receptor…
    /// </summary>
    public static string ToRepoRelative(string path)
    {
        var normalized = path.Replace('/', Path.DirectorySeparatorChar)
            .Replace('\\', Path.DirectorySeparatorChar)
            .Trim();

        // ...\CT_e\... ou ...\CT_e 3.0\... → depois do segmento
        var cteIdx = IndexOfCteSegment(normalized);
        if (cteIdx >= 0)
        {
            var parts = normalized.Split([Path.DirectorySeparatorChar, '/'], StringSplitOptions.None);
            var offset = 0;
            for (var i = 0; i < parts.Length; i++)
            {
                if (IsCteSegment(parts[i]))
                {
                    var after = normalized[(offset + parts[i].Length)..].TrimStart(Path.DirectorySeparatorChar);
                    if (after.Length > 0)
                    {
                        // Se logo após CT_e* vem 0-Orquestrador, prefere a partir dele
                        // (raiz de trabalho unificada).
                        var orqIdx = IndexOfPathSegment(after, "0-Orquestrador");
                        if (orqIdx == 0)
                        {
                            var afterOrq = after["0-Orquestrador".Length..].TrimStart(Path.DirectorySeparatorChar);
                            return string.IsNullOrEmpty(afterOrq) ? string.Empty : afterOrq;
                        }

                        return after;
                    }

                    break;
                }

                offset += parts[i].Length;
                if (i < parts.Length - 1)
                {
                    offset += 1;
                }
            }
        }

        foreach (var marker in RepoMarkers)
        {
            var markerIdx = IndexOfPathSegment(normalized, marker);
            if (markerIdx < 0)
            {
                continue;
            }

            if (string.Equals(marker, "0-Orquestrador", StringComparison.OrdinalIgnoreCase))
            {
                var after = normalized[(markerIdx + marker.Length)..].TrimStart(Path.DirectorySeparatorChar);
                return after;
            }

            return normalized[markerIdx..].TrimStart(Path.DirectorySeparatorChar);
        }

        while (normalized.StartsWith($"..{Path.DirectorySeparatorChar}", StringComparison.Ordinal))
        {
            normalized = normalized[3..];
        }

        return normalized.TrimStart(Path.DirectorySeparatorChar);
    }

    private static IEnumerable<string> EnumerateRelativeCandidates(string repoRoot, string configured)
    {
        var relative = ToRepoRelative(configured);
        yield return Path.GetFullPath(Path.Combine(repoRoot, relative));

        const string orq = "0-Orquestrador";
        var prefix = orq + Path.DirectorySeparatorChar;
        if (relative.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
        {
            yield return Path.GetFullPath(Path.Combine(repoRoot, relative[prefix.Length..]));
        }

        if (!LooksLikeOrquestradorRoot(repoRoot) && LooksLikeMonorepoParent(repoRoot))
        {
            yield return Path.GetFullPath(Path.Combine(repoRoot, orq, relative));
        }
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
            if (LooksLikeOrquestradorRoot(dir.FullName))
            {
                return dir.FullName;
            }

            if (LooksLikeMonorepoParent(dir.FullName))
            {
                var orq = Path.Combine(dir.FullName, "0-Orquestrador");
                if (LooksLikeOrquestradorRoot(orq))
                {
                    return orq;
                }

                return dir.FullName;
            }

            dir = dir.Parent;
        }

        return null;
    }

    private static string NormalizeRoot(string path)
    {
        if (LooksLikeOrquestradorRoot(path))
        {
            return path;
        }

        if (LooksLikeMonorepoParent(path))
        {
            var orq = Path.Combine(path, "0-Orquestrador");
            if (LooksLikeOrquestradorRoot(orq))
            {
                return orq;
            }
        }

        return path;
    }

    private static bool LooksLikeRepoRoot(string path) =>
        LooksLikeOrquestradorRoot(path) || LooksLikeMonorepoParent(path);

    private static bool LooksLikeOrquestradorRoot(string path) =>
        Directory.Exists(Path.Combine(path, "engines"))
        && Directory.Exists(Path.Combine(path, "Orquestrador.Api"));

    private static bool LooksLikeMonorepoParent(string path) =>
        Directory.Exists(Path.Combine(path, "0-Orquestrador"));

    private static bool IsCteSegment(string segment) =>
        segment.Equals("CT_e", StringComparison.OrdinalIgnoreCase)
        || segment.StartsWith("CT_e ", StringComparison.OrdinalIgnoreCase)
        || segment.StartsWith("CT_e_", StringComparison.OrdinalIgnoreCase);

    private static int IndexOfCteSegment(string path)
    {
        var parts = path.Split([Path.DirectorySeparatorChar, '/'], StringSplitOptions.None);
        var offset = 0;
        for (var i = 0; i < parts.Length; i++)
        {
            if (IsCteSegment(parts[i]))
            {
                return offset;
            }

            offset += parts[i].Length;
            if (i < parts.Length - 1)
            {
                offset += 1;
            }
        }

        return -1;
    }

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
                offset += 1;
            }
        }

        return -1;
    }
}
