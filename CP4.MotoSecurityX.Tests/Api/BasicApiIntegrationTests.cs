using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

public class BasicApiIntegrationTests : IClassFixture<WebApplicationFactory<CP4.MotoSecurityX.Api.Program>>
{
    private readonly WebApplicationFactory<CP4.MotoSecurityX.Api.Program> _factory;
    private const string ApiKey = "MINHA_CHAVE_SUPER_SECRETA";

    public BasicApiIntegrationTests(WebApplicationFactory<CP4.MotoSecurityX.Api.Program> factory)
    {
        _factory = factory.WithWebHostBuilder(_ => { /* appsettings de teste se precisar */ });
    }

    [Fact]
    public async Task Health_DeveRetornar200()
    {
        var client = _factory.CreateClient();
        var resp = await client.GetAsync("/health");
        Assert.True(resp.IsSuccessStatusCode);
    }

    [Fact]
    public async Task ML_Sentiment_DeveRetornar200_QuandoComApiKey()
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("X-API-KEY", ApiKey);
        var payload = new StringContent(JsonSerializer.Serialize(new { text = "ótimo" }), Encoding.UTF8, "application/json");
        var resp = await client.PostAsync("/api/v1/ml/sentiment", payload);
        Assert.True(resp.IsSuccessStatusCode);
    }

    [Fact]
    public async Task ML_Sentiment_DeveRetornar401_SemApiKey()
    {
        var client = _factory.CreateClient();
        var payload = new StringContent("""{"text":"texto"}""", Encoding.UTF8, "application/json");
        var resp = await client.PostAsync("/api/v1/ml/sentiment", payload);
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, resp.StatusCode);
    }
}