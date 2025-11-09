namespace CP4.MotoSecurityX.Application.Services
{
    public interface ISentimentAnalyzer
    {
        Task<(bool isPositive, float score)> AnalyzeAsync(string text, CancellationToken ct = default);
    }
}