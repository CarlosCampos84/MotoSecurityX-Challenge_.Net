using CP4.MotoSecurityX.Domain.Repositories;
using CP4.MotoSecurityX.Infrastructure.Data;
using CP4.MotoSecurityX.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CP4.MotoSecurityX.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            var cs = config.GetConnectionString("Default") ?? "Data Source=motosecurityx.db";

            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlite(cs);
            });

            services.AddScoped<IMotoRepository, MotoRepository>();
            services.AddScoped<IPatioRepository, PatioRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();

            return services;
        }
    }
}

