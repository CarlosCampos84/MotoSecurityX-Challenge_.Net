using System.ComponentModel.DataAnnotations;

namespace CP4.MotoSecurityX.Api.Models.ML
{
    public sealed class SentimentRequest
    {
        [Required]
        public string Text { get; set; } = string.Empty;
    }
}