namespace CTe.Modules.Monitors.WindowsControl;

/// <summary>
/// Descobre a raiz de trabalho do Orquestrador sem depender do prefixo absoluto da máquina
/// (C:\Users\..., D:\Clones\..., nome CT_e / CT_e 2.0 / CT_e 3.0).
/// Preferência: pasta <c>0-Orquestrador</c> (tem <c>engines</c> + <c>Orquestrador.Api</c>);
/// fallback: pasta pai que contém <c>0-Orquestrador</c> (monorepo clássico).
/// </summary>
public static class RepoRootResolver
{
    public static string? FindRepoRoot(string? configuredRootPath, params string?[] searchStarts)
    {
        if (!string.IsNullOrWhiteSpace(configuredRootPath))
        {
            try
            {
                var configured = Path.GetFullPath(configuredRootPath.Trim());
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

        return null;
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

        // repoRoot = pai do monorepo; package já é engines\X
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

    /// <summary>0-Orquestrador unificado (engines embutidos).</summary>
    private static bool LooksLikeOrquestradorRoot(string path) =>
        Directory.Exists(Path.Combine(path, "engines"))
        && Directory.Exists(Path.Combine(path, "Orquestrador.Api"));

    /// <summary>Pai do monorepo (qualquer nome: CT_e, CT_e 3.0, …).</summary>
    private static bool LooksLikeMonorepoParent(string path) =>
        Directory.Exists(Path.Combine(path, "0-Orquestrador"));
}
