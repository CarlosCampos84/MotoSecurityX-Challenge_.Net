namespace CP4.MotoSecurityX.Application.DTOs.ML
{
    // Resultado que o AnalyzeSentimentHandler retorna
    public sealed record SentimentResult(bool IsPositive, float Score);
}