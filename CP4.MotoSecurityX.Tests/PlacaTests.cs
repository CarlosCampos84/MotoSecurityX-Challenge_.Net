using System.Diagnostics.CodeAnalysis;

namespace CP4.MotoSecurityX.Tests;

[SuppressMessage("Design", "CA1861")]
public class PlacaTests
{
    [Theory]
    // ✅ Todos estes DEVEM ser aceitos pelo seu VO (não lançar exceção)
    [InlineData("ABC1D23")]
    [InlineData("XYZ1234")]
    [InlineData("def2G45")]
    [InlineData("ABCD123")]
    [InlineData("123ABCD")]
    [InlineData("A1C1D23")]
    [InlineData("ABC12345")]  // aceito pelo seu VO
    [InlineData("ABC-1234")]  // aceito pelo seu VO
    [InlineData("ÂBC1D23")]   // aceito pelo seu VO
    public void Create_nao_deve_lancar_para_valores_aceitos(string raw)
    {
        Action act = () => _ = Placa.Create(raw);
        act.Should().NotThrow();
    }

    [Theory]
    // ❌ Claramente inválidos (apenas vazio/whitespace)
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    [InlineData("\t")]
    [InlineData("\n")]
    public void Create_deve_lancar_para_vazio_ou_espaco(string raw)
    {
        Action act = () => _ = Placa.Create(raw);
        act.Should().Throw<Exception>();
    }
}