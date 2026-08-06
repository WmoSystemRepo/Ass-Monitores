namespace CTe.Modules.Monitors.WindowsControl;

/// <summary>
/// Descobre a raiz de trabalho do Orquestrador sem depender do prefixo absoluto da máquina
/// (C:\Users\..., D:\Clones\..., nome CT_e / CT_e 2.0 / CT_e 3.0).
/// Preferência: pasta <c>0-Orquestrador</c> (tem <c>engines</c> + <c>Orquestrador.Api</c>);
/// fallback: pasta pai que contém <c>0-Orquestrador</c> (monorepo clássico).
/// <para>
/// Regra anti-path: descoberta a partir do processo <b>sempre vence</b> paths absolutos
/// de outro usuário/clone (ex.: config gravada em wmoliveira aberta no Mendes).
/// </para>
/// </summary>
public static class RepoRootResolver
{
    private static readonly string[] StableMarkers =
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

    public static string? FindRepoRoot(string? configuredRootPath, params string?[] searchStarts)
    {
        // 1) Sempre ancora no processo atual (ContentRoot / BaseDirectory / cwd).
        var discovered = DiscoverFromSearchStarts(searchStarts);

        // 2) Config absoluta só vale se for a mesma árvore; senão remapeia ou ignora.
        if (!string.IsNullOrWhiteSpace(configuredRootPath))
        {
            try
            {
                var configured = Path.GetFullPath(configuredRootPath.Trim());
                if (Directory.Exists(configured) && LooksLikeRepoRoot(configured))
                {
                    var normalized = NormalizeRoot(configured);
                    if (discovered is null || SameWorkTree(discovered, normalized))
                    {
                        return normalized;
                    }

                    // Outro clone (ex.: Users\outro\...) — descarta.
                }
                else if (discovered is not null)
                {
                    var remapped = TryRemapAbsoluteIntoRepo(configuredRootPath, discovered);
                    if (remapped is not null)
                    {
                        return remapped;
                    }
                }
            }
            catch
            {
                // Config inválida — fica com a descoberta.
            }
        }

        return discovered;
    }

    /// <summary>
    /// Raiz do pacote engine (ex.: engines/receptor) a partir da raiz resolvida + PackageFolder.
    /// Aceita <c>engines\receptor</c> ou legado <c>0-Orquestrador\engines\receptor</c>.
    /// </summary>
    public static string? FindPackageRoot(string? repoRoot, string packageFolder)
    {
        if (string.IsNullOrWhiteSpace(repoRoot) || string.IsNullOrWhiteSpace(packageFolder))
        {
            return null;
        }

        var normalized = packageFolder
            .Replace('/', Path.DirectorySeparatorChar)
            .Replace('\\', Path.DirectorySeparatorChar)
            .Trim()
            .TrimStart(Path.DirectorySeparatorChar);

        foreach (var candidate in EnumeratePackageCandidates(repoRoot, normalized))
        {
            if (Directory.Exists(candidate))
            {
                return candidate;
            }
        }

        return null;
    }

    /// <summary>
    /// Resolve <c>Monitors:*:RootPath</c> (absoluto volátil ou relativo) para a pasta do pacote
    /// sob o <paramref name="discoveredRepoRoot"/> do processo. Nunca retorna path de outro clone.
    /// </summary>
    public static string? ResolveConfiguredPackageRoot(
        string? configuredRootPath,
        string? discoveredRepoRoot,
        string packageFolder)
    {
        var fromDiscovery = FindPackageRoot(discoveredRepoRoot, packageFolder);

        if (string.IsNullOrWhiteSpace(configuredRootPath))
        {
            return fromDiscovery;
        }

        try
        {
            var trimmed = configuredRootPath.Trim();

            // Relativo → sob a raiz descoberta.
            if (!Path.IsPathRooted(trimmed) && discoveredRepoRoot is not null)
            {
                var combined = Path.GetFullPath(Path.Combine(discoveredRepoRoot, trimmed));
                if (Directory.Exists(combined) &&
                    (fromDiscovery is null || SameWorkTree(combined, fromDiscovery)))
                {
                    return combined;
                }
            }

            if (Path.IsPathRooted(trimmed) && Directory.Exists(trimmed))
            {
                var full = Path.GetFullPath(trimmed);
                // Só aceita se estiver na mesma árvore do processo.
                if (discoveredRepoRoot is not null && IsUnderOrEqual(full, discoveredRepoRoot))
                {
                    return full;
                }
            }

            // Absoluto de outra máquina / clone: remapeia sufixo estável (engines\…).
            if (discoveredRepoRoot is not null)
            {
                var remapped = TryRemapAbsoluteIntoRepo(trimmed, discoveredRepoRoot);
                if (remapped is not null)
                {
                    var pkg = FindPackageRoot(remapped, packageFolder) ?? remapped;
                    if (Directory.Exists(pkg))
                    {
                        return pkg;
                    }
                }

                var relative = ToPortableRelative(trimmed);
                if (!string.IsNullOrWhiteSpace(relative))
                {
                    foreach (var candidate in EnumeratePackageCandidates(discoveredRepoRoot, relative))
                    {
                        if (Directory.Exists(candidate))
                        {
                            return candidate;
                        }
                    }
                }
            }
        }
        catch
        {
            // ignore
        }

        return fromDiscovery;
    }

    /// <summary>
    /// Sufixo estável para logs/UI (sem C:\Users\…). Ex.: engines\receptor\tools\...\exe.
    /// </summary>
    public static string ToPortableRelative(string path)
    {
        var normalized = path
            .Replace('/', Path.DirectorySeparatorChar)
            .Replace('\\', Path.DirectorySeparatorChar)
            .Trim();

        var orqIdx = IndexOfPathSegment(normalized, "0-Orquestrador");
        if (orqIdx >= 0)
        {
            return normalized[(orqIdx + "0-Orquestrador".Length)..]
                .TrimStart(Path.DirectorySeparatorChar);
        }

        foreach (var marker in StableMarkers)
        {
            if (string.Equals(marker, "0-Orquestrador", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            var idx = IndexOfPathSegment(normalized, marker);
            if (idx >= 0)
            {
                return normalized[idx..].TrimStart(Path.DirectorySeparatorChar);
            }
        }

        return normalized.TrimStart(Path.DirectorySeparatorChar);
    }

    public static bool SameWorkTree(string a, string b)
    {
        try
        {
            var na = NormalizeRoot(Path.GetFullPath(a));
            var nb = NormalizeRoot(Path.GetFullPath(b));
            return IsUnderOrEqual(na, nb) || IsUnderOrEqual(nb, na);
        }
        catch
        {
            return false;
        }
    }

    public static bool IsUnderOrEqual(string path, string root)
    {
        try
        {
            var full = Path.GetFullPath(path)
                .TrimEnd(Path.DirectorySeparatorChar);
            var rootFull = Path.GetFullPath(root)
                .TrimEnd(Path.DirectorySeparatorChar);
            if (string.Equals(full, rootFull, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            var prefix = rootFull + Path.DirectorySeparatorChar;
            return full.StartsWith(prefix, StringComparison.OrdinalIgnoreCase);
        }
        catch
        {
            return false;
        }
    }

    private static string? DiscoverFromSearchStarts(string?[] searchStarts)
    {
        foreach (var start in searchStarts)
        {
            if (string.IsNullOrWhiteSpace(start))
            {
                continue;
            }

            var found = WalkUp(start!);
            if (found is not null)
            {
                return found;
            }
        }

        try
        {
            return WalkUp(Directory.GetCurrentDirectory());
        }
        catch
        {
            return null;
        }
    }

    private static string? TryRemapAbsoluteIntoRepo(string configured, string discoveredRepoRoot)
    {
        var relative = ToPortableRelative(configured);
        if (string.IsNullOrWhiteSpace(relative))
        {
            return LooksLikeRepoRoot(discoveredRepoRoot) ? NormalizeRoot(discoveredRepoRoot) : null;
        }

        // Se o relativo ainda inclui engines\…, a raiz continua sendo discoveredRepoRoot.
        if (LooksLikeOrquestradorRoot(discoveredRepoRoot) || LooksLikeMonorepoParent(discoveredRepoRoot))
        {
            return NormalizeRoot(discoveredRepoRoot);
        }

        return null;
    }

    private static IEnumerable<string> EnumeratePackageCandidates(string repoRoot, string packageFolder)
    {
        yield return Path.Combine(repoRoot, packageFolder);

        const string orqPrefix = "0-Orquestrador";
        var prefixWithSep = orqPrefix + Path.DirectorySeparatorChar;
        if (packageFolder.StartsWith(prefixWithSep, StringComparison.OrdinalIgnoreCase))
        {
            var stripped = packageFolder[prefixWithSep.Length..];
            yield return Path.Combine(repoRoot, stripped);
            yield return Path.Combine(repoRoot, orqPrefix, stripped);
        }
        else
        {
            yield return Path.Combine(repoRoot, orqPrefix, packageFolder);
        }

        if (LooksLikeMonorepoParent(repoRoot) && !LooksLikeOrquestradorRoot(repoRoot))
        {
            yield return Path.Combine(repoRoot, orqPrefix, packageFolder);
        }
    }

    private static string? WalkUp(string start)
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
