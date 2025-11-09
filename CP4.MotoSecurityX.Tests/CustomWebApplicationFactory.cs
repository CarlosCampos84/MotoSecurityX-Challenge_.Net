using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace CP4.MotoSecurityX.Tests
{
    public sealed class CustomWebApplicationFactory
        : WebApplicationFactory<CP4.MotoSecurityX.Api.Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureAppConfiguration((ctx, cfg) =>
            {
                // Injeta configurações em memória para os testes
                cfg.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    // força a API a aceitar a mesma chave que o teste envia
                    ["ApiKeyAuth:ApiKey"] = "test-api-key-123",
                    ["ApiKeyAuth:ApiKeyHeaderName"] = "X-API-KEY",

                    // Você pode fixar UseMongo=false nos testes, se quiser,
                    // evitando dependência de Mongo local.
                    ["UseMongo"] = "false",

                    // ConnectionString fallback (se precisar rodar Health/SQLite)
                    ["ConnectionStrings:Default"] = "Data Source=:memory:" // ou motosecurityx_test.db
                });
            });
        }
    }
}