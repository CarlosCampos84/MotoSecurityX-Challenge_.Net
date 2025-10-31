using HealthChecks.MongoDb;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

using CP4.MotoSecurityX.Application.UseCases.Motos;
using CP4.MotoSecurityX.Application.UseCases.Patios;
using CP4.MotoSecurityX.Application.UseCases.Usuarios;
using CP4.MotoSecurityX.Infrastructure;
using CP4.MotoSecurityX.Infrastructure.Mongo;

var builder = WebApplication.CreateBuilder(args);

// -------- Controllers + Explorer --------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureOptions<SwaggerGenOptionsSetup>();

// -------- API Versioning --------
builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new ApiVersion(1, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true;
    opt.ApiVersionReader = new UrlSegmentApiVersionReader(); // /v{version}/...
});
builder.Services.AddVersionedApiExplorer(opt =>
{
    opt.GroupNameFormat = "'v'VVV";
    opt.SubstituteApiVersionInUrl = true;
});

// -------- Swagger --------
builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MotoSecurityX.Api",
        Version = "v1",
        Description = "API para controle de motos, pátios e usuários (Clean Architecture + DDD)"
    });

    o.EnableAnnotations();        // Swashbuckle.AspNetCore.Annotations
    o.ExampleFilters();           // Swashbuckle.AspNetCore.Filters
});
builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();

// -------- Infra: EF/SQLite OU Mongo (via configuração UseMongo) --------
var useMongo = builder.Configuration.GetValue<bool>("UseMongo");

if (useMongo)
{
    // DI do Mongo (MongoOptions, MongoContext e repositórios)
    builder.Services.AddMongoInfrastructure(builder.Configuration);
}
else
{
    // EF/SQLite
    builder.Services.AddInfrastructure(builder.Configuration);
}

// -------- HealthChecks --------
var hc = builder.Services.AddHealthChecks();

// Liveness (processo de pé)
hc.AddCheck("self", () => HealthCheckResult.Healthy(), tags: new[] { "live" });

if (useMongo)
{
    // Readiness de MongoDB por connection string (não requer MongoDB.Driver)
    var mongoConn = builder.Configuration["Mongo:ConnectionString"] ?? "mongodb://localhost:27017";
    hc.AddMongoDb(
        mongoConn,
        name: "mongodb",
        tags: new[] { "ready" }
    );
}
else
{
    // Readiness real de SQLite (AspNetCore.HealthChecks.Sqlite)
    var sqliteConn = builder.Configuration.GetConnectionString("DefaultConnection")
                     ?? "Data Source=app.db";
    hc.AddSqlite(
        sqliteConn,
        name: "sqlite",
        tags: new[] { "ready" }
    );
}

// -------- Application Handlers --------
// Motos
builder.Services.AddScoped<CreateMotoHandler>();
builder.Services.AddScoped<GetMotoByIdHandler>();
builder.Services.AddScoped<ListMotosHandler>();
builder.Services.AddScoped<MoveMotoToPatioHandler>();
builder.Services.AddScoped<UpdateMotoHandler>();
builder.Services.AddScoped<DeleteMotoHandler>();
// Pátios
builder.Services.AddScoped<CreatePatioHandler>();
builder.Services.AddScoped<GetPatioByIdHandler>();
builder.Services.AddScoped<ListPatiosHandler>();
builder.Services.AddScoped<UpdatePatioHandler>();
builder.Services.AddScoped<DeletePatioHandler>();
// Usuários
builder.Services.AddScoped<CreateUsuarioHandler>();
builder.Services.AddScoped<GetUsuarioByIdHandler>();
builder.Services.AddScoped<ListUsuariosHandler>();
builder.Services.AddScoped<UpdateUsuarioHandler>();
builder.Services.AddScoped<DeleteUsuarioHandler>();

var app = builder.Build();

// -------- Middleware --------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// -------- Endpoints de Health (separados por tags) --------
app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = r => r.Tags.Contains("live"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = r => r.Tags.Contains("ready"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// Rota agregada simples (/health)
app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// -------- Rotas --------
app.MapControllers();
app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();
