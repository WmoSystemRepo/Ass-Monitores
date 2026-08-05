using CTe.Resgate.Application.Services;

namespace CTe.Resgate.Domain.Tests;

public sealed class ChavePlanilhaParserTests
{
    [Fact]
    public void ParseTxt_ignora_cabecalho_e_extrai_chaves()
    {
        var text = "chave\n35260712345678901234567890123456789012345678\n";
        using var ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(text));
        var keys = ChavePlanilhaParser.Parse(ms, ".txt").ToList();
        Assert.Single(keys);
        Assert.Equal(44, keys[0].Length);
    }

    [Fact]
    public void ParseCsv_pega_primeira_coluna()
    {
        var text = "35260712345678901234567890123456789012345678;obs\n";
        using var ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(text));
        var keys = ChavePlanilhaParser.Parse(ms, ".csv").ToList();
        Assert.Single(keys);
        Assert.True(keys[0].All(char.IsDigit));
    }
}
