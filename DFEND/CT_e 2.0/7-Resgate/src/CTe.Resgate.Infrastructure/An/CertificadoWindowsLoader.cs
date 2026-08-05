using System.Security.Cryptography.X509Certificates;

namespace CTe.Resgate.Infrastructure.An;

public static class CertificadoWindowsLoader
{
    /// <summary>
    /// Localiza certificado A1 no store Windows (CurrentUser, depois LocalMachine)
    /// cujo Subject contém o texto configurado (ex.: BAHIA SECRETARIA DA FAZENDA).
    /// </summary>
    public static X509Certificate2? FindBySubjectContains(string subjectFragment)
    {
        if (string.IsNullOrWhiteSpace(subjectFragment))
            return null;

        foreach (var location in new[] { StoreLocation.CurrentUser, StoreLocation.LocalMachine })
        {
            var cert = FindInStore(location, subjectFragment);
            if (cert is not null)
                return cert;
        }

        return null;
    }

    private static X509Certificate2? FindInStore(StoreLocation location, string subjectFragment)
    {
        using var store = new X509Store(StoreName.My, location);
        try
        {
            store.Open(OpenFlags.ReadOnly);
        }
        catch
        {
            return null;
        }

        foreach (var cert in store.Certificates.Cast<X509Certificate2>())
        {
            if (cert.Subject is null
                || cert.Subject.IndexOf(subjectFragment, StringComparison.OrdinalIgnoreCase) < 0)
                continue;

            try
            {
                if (!cert.HasPrivateKey)
                    continue;
            }
            catch
            {
                continue;
            }

            // Clone para uso fora do store (HttpClientHandler descarta após uso).
            var exported = cert.Export(X509ContentType.Pkcs12);
            return new X509Certificate2(exported, (string?)null, X509KeyStorageFlags.Exportable);
        }

        return null;
    }
}
