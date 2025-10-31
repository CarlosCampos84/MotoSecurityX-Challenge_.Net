namespace CP4.MotoSecurityX.Domain.ValueObjects;

public sealed class Placa : IEquatable<Placa>
{
    public string Value { get; private set; }

    // Construtor sem parâmetros para o EF Core
    private Placa() { Value = null!; }

    // Construtor privado para garantir uso da fábrica
    private Placa(string value) => Value = value;

    public static Placa Create(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
            throw new ArgumentException("Placa não pode ser vazia.", nameof(raw));

        // Normalização: tira caracteres não alfanuméricos e padroniza
        var norm = new string(raw.Trim().ToUpperInvariant()
            .Where(char.IsLetterOrDigit).ToArray());

        if (norm.Length is < 7 or > 8)
            throw new ArgumentException("Placa em formato inválido.", nameof(raw));

        return new Placa(norm);
    }

    public override string ToString() => Value;

    public bool Equals(Placa? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is Placa p && Equals(p);
    public override int GetHashCode() => Value.GetHashCode(StringComparison.Ordinal);
}




