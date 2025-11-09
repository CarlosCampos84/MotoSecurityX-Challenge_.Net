using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CP4.MotoSecurityX.Api.Configuration
{
    internal sealed class SwaggerGenOptionsSetup : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;
        private const string ApiKeySchemeId = "ApiKey";

        public SwaggerGenOptionsSetup(IApiVersionDescriptionProvider provider)
            => _provider = provider;

        public void Configure(SwaggerGenOptions options)
        {
            // Documentos por versão descoberta (v1, v2, ...)
            foreach (var desc in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(desc.GroupName, new OpenApiInfo
                {
                    Title = "MotoSecurityX API",
                    Version = desc.ApiVersion.ToString(),
                    Description = "API para controle de motos, pátios e usuários (Clean Architecture + DDD)"
                });
            }

            // Anotações + exemplos
            options.EnableAnnotations();
            options.ExampleFilters();

            // Segurança por API Key (header)
            options.AddSecurityDefinition(ApiKeySchemeId, new OpenApiSecurityScheme
            {
                Name = "X-API-KEY",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Description = "Informe a chave de acesso da API no header 'X-API-KEY'."
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme, Id = ApiKeySchemeId
                        }
                    },
                    Array.Empty<string>()
                }
            });
        }
    }
}
