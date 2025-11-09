using System;
using CP4.MotoSecurityX.Api.Options;
using Microsoft.Extensions.Options;

namespace CP4.MotoSecurityX.Api.Security
{
    public sealed class ApiKeyValidator : IApiKeyValidator
    {
        private readonly ApiKeyAuthOptions _opt;

        public ApiKeyValidator(IOptions<ApiKeyAuthOptions> opt)
        {
            _opt = opt.Value ?? new ApiKeyAuthOptions();
        }

        public bool IsValid(string? provided)
        {
            // fallback seguro aos defaults caso não venham via config
            var expected = string.IsNullOrWhiteSpace(_opt.ApiKey)
                ? "MINHA_CHAVE_SUPER_SECRETA"
                : _opt.ApiKey;

            // comparação exata; header é case-insensitive no nome, mas o valor não
            return string.Equals(provided?.Trim(), expected, StringComparison.Ordinal);
        }
    }
}