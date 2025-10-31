namespace CP4.MotoSecurityX.Domain.Entities;

public class Usuario
{
    private Usuario() { } // EF

    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Nome { get; private set; } = "";
    public string Email { get; private set; } = "";

    public Usuario(string nome, string email)
    {
        AtualizarNome(nome);
        AtualizarEmail(email);
    }

    public void AtualizarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome)) throw new ArgumentException("Nome inválido");
        Nome = nome.Trim();
    }

    public void AtualizarEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            throw new ArgumentException("Email inválido");
        Email = email.Trim();
    }
}



