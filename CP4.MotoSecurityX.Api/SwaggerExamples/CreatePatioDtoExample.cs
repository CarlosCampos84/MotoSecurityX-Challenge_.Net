namespace CP4.MotoSecurityX.Api.SwaggerExamples;

using CP4.MotoSecurityX.Application.DTOs;
using Swashbuckle.AspNetCore.Filters;

public sealed class CreatePatioDtoExample : IExamplesProvider<CreatePatioDto>
{
    public CreatePatioDto GetExamples()
    {
        return new CreatePatioDto();
        // Melhor:
        // return new CreatePatioDto { Nome = "Pátio Central", Endereco = "Av. Principal, 123" };
    }
}