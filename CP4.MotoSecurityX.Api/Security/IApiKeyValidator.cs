namespace CP4.MotoSecurityX.Api.Security
{
    public interface IApiKeyValidator
    {
        bool IsValid(string? provided);
    }
}