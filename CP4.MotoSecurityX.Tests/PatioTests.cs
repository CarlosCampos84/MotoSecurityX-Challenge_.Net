namespace CP4.MotoSecurityX.Tests;

public class PatioTests
{
    [Fact]
    public void AdmitirMoto_nao_deve_duplicar_nem_reatribuir_se_ja_estiver_no_patio()
    {
        var patio = new Patio("Pátio A", "Rua 1");
        var moto = new Moto(Placa.Create("ABC1D23"), "CG 160");

        patio.AdmitirMoto(moto);
        var countAntes = patio.Motos.Count;

        // repetir a admissão da mesma moto
        patio.AdmitirMoto(moto);

        patio.Motos.Count.Should().Be(countAntes);
        moto.DentroDoPatio.Should().BeTrue();
        moto.PatioId.Should().Be(patio.Id);
    }

    [Fact]
    public void RemoverMoto_deve_tirar_do_patio_e_atualizar_flags_da_moto()
    {
        var patio = new Patio("Pátio A", "Rua 1");
        var moto = new Moto(Placa.Create("XYZ2H34"), "CG 160");

        patio.AdmitirMoto(moto);
        patio.RemoverMoto(moto);

        patio.Motos.Should().BeEmpty();
        moto.DentroDoPatio.Should().BeFalse();
        moto.PatioId.Should().BeNull();
    }
}