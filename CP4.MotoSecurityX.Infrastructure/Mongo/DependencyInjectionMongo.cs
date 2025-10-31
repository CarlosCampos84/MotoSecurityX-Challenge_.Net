using CP4.MotoSecurityX.Domain.Repositories;
using CP4.MotoSecurityX.Infrastructure.Mongo.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CP4.MotoSecurityX.Infrastructure.Mongo
{
    public static class DependencyInjectionMongo
    {
        public static IServiceCollection AddMongoInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            // 1) Options (ConnectionString / DatabaseName)
            services.Configure<MongoOptions>(opt =>
            {
                opt.ConnectionString = config["Mongo:ConnectionString"] ?? "mongodb://localhost:27017";
                opt.DatabaseName     = config["Mongo:DatabaseName"]     ?? "motosecurityx";
            });

            // 2) MongoClient (Singleton)
            services.AddSingleton<IMongoClient>(sp =>
            {
                var opts = sp.GetRequiredService<IOptions<MongoOptions>>().Value;
                return new MongoClient(opts.ConnectionString);
            });

            // 3) Contexto (Scoped) — importante registrar pela interface usada nos repositórios
            services.AddScoped<IMongoContext, MongoContext>();

            // 4) Repositórios (ajuste dos nomes corretos)
            services.AddScoped<IMotoRepository, MotoMongoRepository>();
            services.AddScoped<IPatioRepository, PatioMongoRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioMongoRepository>();

            return services;
        }
    }
}

