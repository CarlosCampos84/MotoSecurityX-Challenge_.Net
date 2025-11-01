namespace CP4.MotoSecurityX.Api.SwaggerExamples;

using CP4.MotoSecurityX.Application.DTOs;
using Swashbuckle.AspNetCore.Filters;

public sealed class UpdateUsuarioDtoExample : IExamplesProvider<UpdateUsuarioDto>
{
    public UpdateUsuarioDto GetExamples()
    {
        return new UpdateUsuarioDto();
        // Exemplo melhor:
        // return new UpdateUsuarioDto { Nome = "Operador João Atualizado", Email = "joao.atualizado@mottu.com" };
    }
}