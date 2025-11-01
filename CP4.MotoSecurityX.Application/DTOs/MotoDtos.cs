using System.ComponentModel.DataAnnotations;
namespace CP4.MotoSecurityX.Application.DTOs;

public class MotoDto
{
    public Guid Id { get; set; }
    public string Placa { get; set; } = "";
    public string Modelo { get; set; } = "";
    public bool DentroDoPatio { get; set; }         
    public Guid? PatioId { get; set; }
}

public class CreateMotoDto
{
    [Required, StringLength(8, MinimumLength = 7)]
    public string Placa { get; set; } = "";

    [Required, StringLength(120)]
    public string Modelo { get; set; } = "";
}

public class UpdateMotoDto
{
    [Required, StringLength(8, MinimumLength = 7)]
    public string Placa { get; set; } = "";

    [Required, StringLength(120)]
    public string Modelo { get; set; } = "";
}

public class MoveMotoDto
{
    [Required]
    public Guid PatioId { get; set; }
}