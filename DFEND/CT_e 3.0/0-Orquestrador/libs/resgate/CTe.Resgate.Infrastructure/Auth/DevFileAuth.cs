namespace CTe.Resgate.Infrastructure.Auth;

/// <summary>
/// Autenticação DEV provisória: valida usuário/senha em um .txt (usuario:senha).
/// Fallback: Auth:Usuario / Auth:Senha do appsettings.
/// </summary>
public static class DevFileAuth
{
    public static bool TryValidate(
        string? usuario,
        string? senha,
        string? usersFilePath,
        string? fallbackUsuario,
        string? fallbackSenha,
        out string? matchedUser)
    {
        matchedUser = null;
        var u = (usuario ?? string.Empty).Trim();
        var p = senha ?? string.Empty;
        if (u.Length == 0)
            return false;

        var file = ResolveUsersFile(usersFilePath);
        if (file is not null && File.Exists(file))
        {
            foreach (var raw in File.ReadLines(file))
            {
                var line = raw.Trim();
                if (line.Length == 0 || line.StartsWith('#'))
                    continue;

                var sep = line.IndexOf(':');
                if (sep <= 0)
                    continue;

                var fileUser = line[..sep].Trim();
                var filePass = line[(sep + 1)..];
                if (string.Equals(fileUser, u, StringComparison.OrdinalIgnoreCase)
                    && string.Equals(filePass, p, StringComparison.Ordinal))
                {
                    matchedUser = fileUser;
                    return true;
                }
            }
        }

        var fbUser = string.IsNullOrWhiteSpace(fallbackUsuario) ? "dev" : fallbackUsuario.Trim();
        var fbPass = string.IsNullOrWhiteSpace(fallbackSenha) ? "dev" : fallbackSenha;
        if (string.Equals(u, fbUser, StringComparison.OrdinalIgnoreCase)
            && string.Equals(p, fbPass, StringComparison.Ordinal))
        {
            matchedUser = fbUser;
            return true;
        }

        return false;
    }

    public static string? ResolveUsersFile(string? configured)
    {
        if (!string.IsNullOrWhiteSpace(configured))
        {
            if (Path.IsPathRooted(configured) && File.Exists(configured))
                return configured;

            var fromCwd = Path.GetFullPath(configured);
            if (File.Exists(fromCwd))
                return fromCwd;

            var fromBase = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, configured));
            if (File.Exists(fromBase))
                return fromBase;
        }

        // Sobe a partir do bin até achar 7-Resgate/data/usuarios-dev.txt
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null)
        {
            var candidate = Path.Combine(dir.FullName, "data", "usuarios-dev.txt");
            if (File.Exists(candidate))
                return candidate;

            if (string.Equals(dir.Name, "7-Resgate", StringComparison.OrdinalIgnoreCase))
            {
                candidate = Path.Combine(dir.FullName, "data", "usuarios-dev.txt");
                if (File.Exists(candidate))
                    return candidate;
            }

            dir = dir.Parent;
        }

        return null;
    }
}
