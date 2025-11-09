using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CP4.MotoSecurityX.Api.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class ApiKeyAuthAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var cfg = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var headerName = cfg["ApiKeyAuth:ApiKeyHeaderName"] ?? "X-API-KEY";
            var expected   = cfg["ApiKeyAuth:ApiKey"];

            if (string.IsNullOrWhiteSpace(expected))
            {
                context.Result = new UnauthorizedObjectResult(new { ok = false, message = "API key não configurada no servidor." });
                return Task.CompletedTask;
            }

            if (!context.HttpContext.Request.Headers.TryGetValue(headerName, out var provided) ||
                !string.Equals(provided.ToString(), expected, StringComparison.Ordinal))
            {
                context.Result = new UnauthorizedObjectResult(new { ok = false, message = "API key inválida ou ausente." });
                return Task.CompletedTask;
            }

            return Task.CompletedTask;
        }
    }
}