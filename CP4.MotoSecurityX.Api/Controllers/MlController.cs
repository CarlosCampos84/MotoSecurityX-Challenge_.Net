using CP4.MotoSecurityX.Api.Models.ML;
using CP4.MotoSecurityX.Application.UseCases.ML;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CP4.MotoSecurityX.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/ml")]
    // Removido [ApiKeyAuth] para evitar dupla validação (middleware já protege tudo)
    public sealed class MlController : ControllerBase
    {
        [HttpPost("sentiment")]
        [SwaggerOperation(Summary = "Analisa sentimento (ML.NET)", Description = "Retorna sentimento positivo/negativo e score.")]
        [ProducesResponseType(typeof(SentimentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SentimentResponse>> Predict(
            [FromBody] SentimentRequest body,
            [FromServices] AnalyzeSentimentHandler handler,
            CancellationToken ct = default)
        {
            if (body is null || string.IsNullOrWhiteSpace(body.Text))
                return BadRequest(new { ok = false, message = "Texto obrigatório." });

            var result = await handler.HandleAsync(body.Text, ct);

            var response = new SentimentResponse
            {
                IsPositive = result.IsPositive,
                Score = result.Score
            };

            return Ok(response);
        }
    }
}