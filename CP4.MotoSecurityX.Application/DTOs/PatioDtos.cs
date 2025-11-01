using System.ComponentModel.DataAnnotations;
namespace CP4.MotoSecurityX.Application.DTOs;

public class PatioDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = "";
    public string Endereco { get; set; } = "";
    // Se você realmente precisa expor um 4º campo (ex.: Capacidade, Ativo, etc.),
    // adicione aqui depois que soubermos qual é. Por ora, mantenha 3 campos.
}

public class CreatePatioDto
{
    [Required, StringLength(120)]
    public string Nome { get; set; } = "";

    [Required, StringLength(200)]
    public string Endereco { get; set; } = "";
}

public class UpdatePatioDto
{
    [Required, StringLength(120)]
    public string Nome { get; set; } = "";

    [Required, StringLength(200)]
    public string Endereco { get; set; } = "";
}