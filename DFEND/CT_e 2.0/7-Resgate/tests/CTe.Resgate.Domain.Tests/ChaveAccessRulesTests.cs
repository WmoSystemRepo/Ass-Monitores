using CTe.Resgate.Domain;

namespace CTe.Resgate.Domain.Tests;

public class ChaveAccessRulesTests
{
    [Fact]
    public void Rejects_invalid_key_and_empty_result()
    {
        var (keys, errors) = ChaveAccessRules.Normalize(["123", "35260712345678901234567890123456789012345678"]);
        Assert.Empty(keys);
        Assert.Contains(errors, e => e.Contains("inválida"));
    }

    [Fact]
    public void Accepts_1_to_1000_and_dedupes()
    {
        var k = "35260712345678901234567890123456789012345678";
        var (keys, errors) = ChaveAccessRules.Normalize([k, k, ""]);
        Assert.Empty(errors);
        Assert.Single(keys);
    }

    [Fact]
    public void Rejects_over_1000()
    {
        var list = Enumerable.Range(0, 1001)
            .Select(i => i.ToString().PadLeft(44, '0'))
            .ToList();
        var (keys, errors) = ChaveAccessRules.Normalize(list);
        Assert.Empty(keys);
        Assert.Contains(errors, e => e.Contains("Máximo"));
    }

    [Fact]
    public void Mask_hides_middle()
    {
        var k = "35260712345678901234567890123456789012345678";
        var m = ChaveAccessRules.Mask(k);
        Assert.StartsWith("352607", m);
        Assert.Contains("****", m);
        Assert.DoesNotContain("123456789012345678901234567890", m);
    }
}
