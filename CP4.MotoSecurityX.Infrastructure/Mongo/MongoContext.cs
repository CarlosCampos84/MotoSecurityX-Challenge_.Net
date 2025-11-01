using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CP4.MotoSecurityX.Infrastructure.Mongo
{
    public interface IMongoContext
    {
        IMongoDatabase Database { get; }
    }

    public sealed class MongoContext : IMongoContext
    {
        public IMongoDatabase Database { get; }

        public MongoContext(IMongoClient client, IOptions<MongoOptions> options)
        {
            var cfg = options.Value ?? throw new ArgumentNullException(nameof(options));
            if (string.IsNullOrWhiteSpace(cfg.DatabaseName))
                throw new ArgumentException("Mongo DatabaseName ausente ou inválido.");

            Database = client.GetDatabase(cfg.DatabaseName);

            // (Opcional) Criar índices aqui
            // CriarIndicesPadrao(Database);
        }
    }
}


