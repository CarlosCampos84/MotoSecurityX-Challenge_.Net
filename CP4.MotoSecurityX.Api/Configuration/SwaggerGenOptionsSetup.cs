using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace CP4.MotoSecurityX.Api.Configuration
{
    internal sealed class SwaggerGenOptionsSetup : IConfigureOptions<SwaggerGenOptions>
    {
        public void Configure(SwaggerGenOptions options)
        {
            // Documento de API para a versão v1
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "MotoSecurityX.Api",
                Version = "v1",
                Description = "API para controle de motos, pátios e usuários (Clean Architecture + DDD)"
            });

            // Ativa anotações e exemplos
            options.EnableAnnotations();
            options.ExampleFilters();
        }
    }
}