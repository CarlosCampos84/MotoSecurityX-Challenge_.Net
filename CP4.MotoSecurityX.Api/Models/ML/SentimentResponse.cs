namespace CP4.MotoSecurityX.Api.Models.ML
{
    public sealed class SentimentResponse
    {
        public bool IsPositive { get; set; }
        public float Score { get; set; }
    }
}