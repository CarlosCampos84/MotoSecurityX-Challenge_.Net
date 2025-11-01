namespace CP4.MotoSecurityX.Api.SwaggerExamples;

using CP4.MotoSecurityX.Application.DTOs;
using Swashbuckle.AspNetCore.Filters;

public sealed class CreateMotoDtoExample : IExamplesProvider<CreateMotoDto>
{
    public CreateMotoDto GetExamples()
    {
        return new CreateMotoDto();
        // Melhor:
        // return new CreateMotoDto { Placa = "ABC1D23", Modelo = "CG 160 Fan" };
    }
}