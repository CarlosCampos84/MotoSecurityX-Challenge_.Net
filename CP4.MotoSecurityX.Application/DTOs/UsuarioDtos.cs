using System.ComponentModel.DataAnnotations;
namespace CP4.MotoSecurityX.Application.DTOs;

public class UsuarioDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = "";
    public string Email { get; set; } = "";
}

public class CreateUsuarioDto
{
    [Required, StringLength(120)]
    public string Nome { get; set; } = "";

    [Required, EmailAddress, StringLength(160)]
    public string Email { get; set; } = "";
}

public class UpdateUsuarioDto
{
    [Required, StringLength(120)]
    public string Nome { get; set; } = "";

    [Required, EmailAddress, StringLength(160)]
    public string Email { get; set; } = "";
}