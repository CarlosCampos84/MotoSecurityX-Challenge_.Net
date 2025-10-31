using CP4.MotoSecurityX.Domain.ValueObjects;

namespace CP4.MotoSecurityX.Domain.Entities;

public class Moto
{
    private Moto() { } // EF

    public Guid Id { get; private set; } = Guid.NewGuid();
    public Placa Placa { get; private set; } = null!;
    public string Modelo { get; private set; } = string.Empty;
    public bool DentroDoPatio { get; private set; }
    public Guid? PatioId { get; private set; }

    public Moto(Placa placa, string modelo)
    {
        Placa = placa ?? throw new ArgumentNullException(nameof(placa));
        AtualizarModelo(modelo);
        DentroDoPatio = false;
    }

    public void EntrarNoPatio(Guid patioId)
    {
        if (patioId == Guid.Empty)
            throw new ArgumentException("PatioId inválido.", nameof(patioId));

        if (DentroDoPatio && PatioId == patioId)
            return;

        PatioId = patioId;
        DentroDoPatio = true;
    }

    public void SairDoPatio()
    {
        if (!DentroDoPatio && PatioId is null)
            return;

        PatioId = null;
        DentroDoPatio = false;
    }

    public void AtualizarModelo(string modelo)
    {
        if (string.IsNullOrWhiteSpace(modelo))
            throw new ArgumentException("Modelo inválido", nameof(modelo));
        Modelo = modelo.Trim();
    }

    public void AtualizarPlaca(string placaRaw)
    {
        Placa = Placa.Create(placaRaw);
    }
}





