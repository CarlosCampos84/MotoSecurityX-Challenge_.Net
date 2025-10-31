using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace CP4.MotoSecurityX.Api.Configuration
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddVersionedSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                // Documento base v1 (você pode adicionar outros depois)
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "MotoSecurityX API",
                    Version = "v1",
                    Description = "API para controle de motos, pátios e usuários (Clean Architecture + DDD)"
                });

                // mantém compatibilidade com seus examples/annotations do Swashbuckle
            });

            return services;
        }

        public static void UseVersionedSwaggerUI(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (var desc in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json",
                        $"MotoSecurityX {desc.GroupName.ToUpperInvariant()}");
                }
            });
        }
    }
}