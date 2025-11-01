namespace CP4.MotoSecurityX.Api.SwaggerExamples;

using CP4.MotoSecurityX.Application.DTOs;
using Swashbuckle.AspNetCore.Filters;

public sealed class MoveMotoDtoExample : IExamplesProvider<MoveMotoDto>
{
    public MoveMotoDto GetExamples()
    {
        return new MoveMotoDto();
        // Melhor (ajuste ao seu contrato real):
        // return new MoveMotoDto { PatioId = Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee") };
        // ou, se seu DTO usa outra propriedade (ex.: PatioDestinoId / Entrar / Sair), ajuste aqui.
    }
}