namespace CP4.MotoSecurityX.Api.SwaggerExamples;

using CP4.MotoSecurityX.Application.DTOs;
using Swashbuckle.AspNetCore.Filters;

public sealed class CreateUsuarioDtoExample : IExamplesProvider<CreateUsuarioDto>
{
    public CreateUsuarioDto GetExamples()
    {
        // QUICK FIX: exemplo mínimo só pra compilar
        return new CreateUsuarioDto();

        // MELHOR (depois de confirmar propriedades do DTO):
        // return new CreateUsuarioDto { Nome = "Operador João", Email = "joao@emottu.com" };
    }
}