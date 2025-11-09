namespace CP4.MotoSecurityX.Tests;

public class MotoTests
{
    [Fact]
    public void Construtor_deve_criar_moto_fora_do_patio()
    {
        var placa = Placa.Create("ABC1D23");

        var moto = new Moto(placa, "CG 160");

        moto.Should().NotBeNull();
        moto.DentroDoPatio.Should().BeFalse();
        moto.PatioId.Should().BeNull();
    }

    [Fact]
    public void AtualizarPlaca_deve_aceitar_valor_valido()
    {
        var moto = new Moto(Placa.Create("ABC1D23"), "CG 160");

        Action act = () => moto.AtualizarPlaca("XYZ2H34");
        act.Should().NotThrow();
    }

    [Fact]
    public void AtualizarPlaca_deve_recusar_valor_invalido()
    {
        var moto = new Moto(Placa.Create("ABC1D23"), "CG 160");

        Action act = () => moto.AtualizarPlaca("123");
        act.Should().Throw<Exception>();
    }

    [Fact]
    public void EntrarNoPatio_deve_setar_flags_e_PatioId()
    {
        var moto = new Moto(Placa.Create("ABC1D23"), "CG 160");
        var patioId = Guid.NewGuid();

        moto.EntrarNoPatio(patioId);

        moto.DentroDoPatio.Should().BeTrue();
        moto.PatioId.Should().Be(patioId);
    }

    [Fact]
    public void SairDoPatio_deve_limpar_PatioId_e_flag()
    {
        var moto = new Moto(Placa.Create("ABC1D23"), "CG 160");
        var patioId = Guid.NewGuid();

        moto.EntrarNoPatio(patioId);
        moto.SairDoPatio();

        moto.DentroDoPatio.Should().BeFalse();
        moto.PatioId.Should().BeNull();
    }
}