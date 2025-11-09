namespace CP4.MotoSecurityX.Api.Options
{
    public sealed class ApiKeyAuthOptions
    {
        /// <summary>
        /// Valor da API Key esperada. Pode ser sobrescrito via appsettings.
        /// </summary>
        public string ApiKey { get; set; } = "MINHA_CHAVE_SUPER_SECRETA";

        /// <summary>
        /// Nome do header que carrega a API Key.
        /// </summary>
        public string ApiKeyHeaderName { get; set; } = "X-API-KEY";
    }
}


