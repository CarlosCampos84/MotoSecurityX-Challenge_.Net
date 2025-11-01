namespace CP4.MotoSecurityX.Api.SwaggerExamples;

using CP4.MotoSecurityX.Application.DTOs;
using Swashbuckle.AspNetCore.Filters;

public sealed class UpdatePatioDtoExample : IExamplesProvider<UpdatePatioDto>
{
    public UpdatePatioDto GetExamples()
    {
        return new UpdatePatioDto();
        // Melhor:
        // return new UpdatePatioDto { Nome = "Pátio Central", Endereco = "Av. Principal, 123" };
    }
}