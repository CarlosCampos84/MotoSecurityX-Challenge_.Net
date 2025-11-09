using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CP4.MotoSecurityX.Tests
{
    public sealed class HealthAndUsersApiTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public HealthAndUsersApiTests(CustomWebApplicationFactory factory)
            => _factory = factory;

        [Fact]
        public async Task Health_Live_Should_Be_OK()
        {
            var client = _factory.CreateClient();
            var resp = await client.GetAsync("/health/live");
            resp.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Swagger_UI_Should_Load()
        {
            var client = _factory.CreateClient();
            var resp = await client.GetAsync("/swagger");
            resp.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Ml_Sentiment_Should_Return_200_With_ApiKey()
        {
            var client = _factory.CreateClient();

            client.DefaultRequestHeaders.Add("X-API-KEY", "test-api-key-123");

            var body = new { text = "serviço excelente e muito bom!" };
            var resp = await client.PostAsJsonAsync("/api/v1/ml/sentiment", body);

            resp.StatusCode.Should().Be(HttpStatusCode.OK);

            var json = await resp.Content.ReadFromJsonAsync<SentimentDto>();
            json.Should().NotBeNull();
            json!.Score.Should().BeGreaterThan(0);
        }

        private sealed class SentimentDto
        {
            public bool IsPositive { get; set; }
            public float Score { get; set; }
        }
    }
}

