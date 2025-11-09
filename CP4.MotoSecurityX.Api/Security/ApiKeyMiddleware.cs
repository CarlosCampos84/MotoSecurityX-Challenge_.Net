using CP4.MotoSecurityX.Api.Options;
using Microsoft.Extensions.Options;

namespace CP4.MotoSecurityX.Api.Security;

public sealed class ApiKeyMiddleware(RequestDelegate next, IApiKeyValidator validator, IOptions<ApiKeyAuthOptions> opt)
{
    public async Task Invoke(HttpContext ctx)
    {
        var headerName = opt.Value.ApiKeyHeaderName;
        var isHealth = ctx.Request.Path.StartsWithSegments("/health");
        var isSwagger = ctx.Request.Path.StartsWithSegments("/swagger");

        // libera health e swagger
        if (isHealth || isSwagger)
        {
            await next(ctx);
            return;
        }

        if (!ctx.Request.Headers.TryGetValue(headerName, out var provided) || !validator.IsValid(provided!))
        {
            ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await ctx.Response.WriteAsync("API Key inválida ou ausente.");
            return;
        }

        await next(ctx);
    }
}