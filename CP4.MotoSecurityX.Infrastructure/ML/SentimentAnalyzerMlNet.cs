using CP4.MotoSecurityX.Application.Services;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace CP4.MotoSecurityX.Infrastructure.ML
{
    public sealed class SentimentAnalyzerMlNet : ISentimentAnalyzer
    {
        private readonly PredictionEngine<Input, Output> _engine;

        public SentimentAnalyzerMlNet()
        {
            var ml = new MLContext();

            var samples = new[]
            {
                new Input{ Text="serviço excelente", Label=true },
                new Input{ Text="muito bom", Label=true },
                new Input{ Text="péssimo atendimento", Label=false },
                new Input{ Text="horrível", Label=false },
            };

            var data = ml.Data.LoadFromEnumerable(samples);
            var pipeline = ml.Transforms.Text.FeaturizeText("Features", nameof(Input.Text))
                .Append(ml.BinaryClassification.Trainers.SdcaLogisticRegression());

            var model = pipeline.Fit(data);
            _engine = ml.Model.CreatePredictionEngine<Input, Output>(model);
        }

        public Task<(bool isPositive, float score)> AnalyzeAsync(string text, CancellationToken ct = default)
        {
            var pred = _engine.Predict(new Input { Text = text });
            return Task.FromResult((pred.Predicted, pred.Score));
        }

        private sealed class Input
        {
            [LoadColumn(0)] public string Text { get; set; } = "";
            [LoadColumn(1)] public bool Label { get; set; }
        }

        private sealed class Output
        {
            [ColumnName("PredictedLabel")] public bool Predicted { get; set; }
            public float Score { get; set; }
        }
    }
}