namespace CP4.MotoSecurityX.Api.SwaggerExamples;

using CP4.MotoSecurityX.Application.DTOs;
using Swashbuckle.AspNetCore.Filters;

public sealed class UpdateMotoDtoExample : IExamplesProvider<UpdateMotoDto>
{
    public UpdateMotoDto GetExamples()
    {
        return new UpdateMotoDto();
        // Melhor (ajuste às props reais do seu DTO; se Placa for imutável, remova):
        // return new UpdateMotoDto { Modelo = "CG 160 Titan" /*, Placa = "ABC1D23"*/ };
    }
}

