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

        public MongoContext(IOptions<MongoOptions> options)
        {
            var cfg = options.Value ?? throw new ArgumentNullException(nameof(options));
            if (string.IsNullOrWhiteSpace(cfg.ConnectionString))
                throw new ArgumentException("Mongo ConnectionString ausente ou inválida.");
            if (string.IsNullOrWhiteSpace(cfg.DatabaseName))
                throw new ArgumentException("Mongo DatabaseName ausente ou inválido.");

            var client = new MongoClient(cfg.ConnectionString);
            Database = client.GetDatabase(cfg.DatabaseName);
        }
    }
}


