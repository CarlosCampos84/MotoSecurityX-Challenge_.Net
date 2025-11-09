using CP4.MotoSecurityX.Application.Services;

namespace CP4.MotoSecurityX.Application.UseCases.ML
{
    public sealed class AnalyzeSentimentHandler
    {
        private readonly ISentimentAnalyzer _analyzer;

        public AnalyzeSentimentHandler(ISentimentAnalyzer analyzer)
            => _analyzer = analyzer;

        public async Task<AnalyzeSentimentResult> HandleAsync(string text, CancellationToken ct = default)
        {
            var (isPositive, score) = await _analyzer.AnalyzeAsync(text, ct);
            return new AnalyzeSentimentResult(isPositive, score);
        }
    }

    public readonly record struct AnalyzeSentimentResult(bool IsPositive, float Score);
}