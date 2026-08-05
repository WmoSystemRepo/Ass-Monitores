namespace CTe.Resgate.Domain;

public sealed class LoteResgate
{
    public long Id { get; set; }
    public string Usuario { get; set; } = "";
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = LoteStatus.Aberto;
    public int Total { get; set; }
    public int Recuperados { get; set; }
    public int Existentes { get; set; }
    public int NaoLocalizados { get; set; }
    public int Erros { get; set; }
    public string? ChaveAtual { get; set; }
    public string? PassoAtualLote { get; set; }
    public Guid CorrelationId { get; set; } = Guid.NewGuid();
}

public sealed class ItemResgate
{
    public long Id { get; set; }
    public long LoteId { get; set; }
    public string Chave { get; set; } = "";
    public string Status { get; set; } = ItemStatus.Pendente;
    public string PassoAtual { get; set; } = PassoResgate.P0;
    public string? Motivo { get; set; }
    public int Tentativas { get; set; }
    public DateTime AtualizadoEm { get; set; } = DateTime.UtcNow;
    public int? TempoMs { get; set; }
}

public sealed class EventoResgate
{
    public long Id { get; set; }
    public long LoteId { get; set; }
    public long? ItemId { get; set; }
    public DateTime Horario { get; set; } = DateTime.UtcNow;
    public string Mensagem { get; set; } = "";
    public string? Passo { get; set; }
}
