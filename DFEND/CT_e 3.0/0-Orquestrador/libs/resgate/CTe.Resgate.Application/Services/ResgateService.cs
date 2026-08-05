using CTe.Resgate.Application.Abstractions;
using CTe.Resgate.Domain;

namespace CTe.Resgate.Application.Services;

/// <summary>
/// Resgate só informa chaves — não implementa download (princípio arquitetural).
/// </summary>
public sealed class ResgateService(ICargaDownloadEnqueue enqueue) : IResgateService
{
    public async Task<(CargaEnqueueResult? Result, IReadOnlyList<string> Errors)> EnfileirarDownloadAsync(
        string usuario, IEnumerable<string> chaves, CancellationToken ct)
    {
        var (keys, errors) = ChaveAccessRules.Normalize(chaves);
        if (errors.Count > 0 || keys.Count == 0)
            return (null, errors);

        var result = await enqueue.EnfileirarAsync(usuario, keys, ct);
        return (result, Array.Empty<string>());
    }

    public Task<object> GetFilaStatusAsync(CancellationToken ct)
        => enqueue.GetFilaStatusAsync(ct);

    public Task<object> GetStatusChavesAsync(IReadOnlyList<string> chaves, CancellationToken ct)
        => enqueue.GetStatusChavesAsync(chaves, ct);
}
