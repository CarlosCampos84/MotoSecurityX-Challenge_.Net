using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Swashbuckle.AspNetCore.Filters;

using CP4.MotoSecurityX.Api.Configuration;
using CP4.MotoSecurityX.Application.UseCases.Motos;
using CP4.MotoSecurityX.Application.UseCases.Patios;
using CP4.MotoSecurityX.Application.UseCases.Usuarios;

// ML – Clean Architecture
using CP4.MotoSecurityX.Application.Services;
using CP4.MotoSecurityX.Application.UseCases.ML;
using CP4.MotoSecurityX.Infrastructure.ML;

using CP4.MotoSecurityX.Infrastructure;
using CP4.MotoSecurityX.Infrastructure.Mongo;

using CP4.MotoSecurityX.Api.Options;
using CP4.MotoSecurityX.Api.Security;

// ---- Mongo (GUID fix compatível) ----
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson;

// registra o serializer de Guid apenas uma vez
static void EnsureGuidSerializer()
{
    try
    {
        BsonSerializer.RegisterSerializer(typeof(Guid), new GuidSerializer(GuidRepresentation.Standard));
    }
    catch (MongoDB.Bson.BsonSerializationException)
    {
        // Já havia um serializer registrado: ignora.
    }
}
EnsureGuidSerializer();

var builder = WebApplication.CreateBuilder(args);

// ---------- Controllers + Explorer ----------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// ---------- Swagger ----------
builder.Services.ConfigureOptions<SwaggerGenOptionsSetup>();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();

// ---------- API Key (Options + Validator) ----------
builder.Services.Configure<ApiKeyAuthOptions>(builder.Configuration.GetSection("ApiKeyAuth"));
builder.Services.AddSingleton<IApiKeyValidator, ApiKeyValidator>();

// ---------- Versionamento ----------
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

// ---------- Infra: SQLite OU Mongo ----------
var useMongo = builder.Configuration.GetValue<bool>("UseMongo");

if (useMongo)
    builder.Services.AddMongoInfrastructure(builder.Configuration);
else
    builder.Services.AddInfrastructure(builder.Configuration);

// ---------- HealthChecks ----------
var hc = builder.Services.AddHealthChecks();

// Liveness
hc.AddCheck("self", () => HealthCheckResult.Healthy(), tags: new[] { "live" });

if (useMongo)
{
    hc.AddMongoDb(
        sp =>
        {
            var cs = builder.Configuration["Mongo:ConnectionString"] ?? "mongodb://localhost:27017";
            return new MongoDB.Driver.MongoClient(cs);
        },
        name: "mongodb",
        tags: new[] { "ready" }
    );
}
else
{
    var sqliteConn = builder.Configuration.GetConnectionString("Default") ?? "Data Source=motosecurityx.db";
    hc.AddSqlite(sqliteConn, name: "sqlite", tags: new[] { "ready" });
}

// ---------- Application Handlers ----------
builder.Services.AddScoped<CreateMotoHandler>();
builder.Services.AddScoped<GetMotoByIdHandler>();
builder.Services.AddScoped<ListMotosHandler>();
builder.Services.AddScoped<MoveMotoToPatioHandler>();
builder.Services.AddScoped<UpdateMotoHandler>();
builder.Services.AddScoped<DeleteMotoHandler>();

builder.Services.AddScoped<CreatePatioHandler>();
builder.Services.AddScoped<GetPatioByIdHandler>();
builder.Services.AddScoped<ListPatiosHandler>();
builder.Services.AddScoped<UpdatePatioHandler>();
builder.Services.AddScoped<DeletePatioHandler>();

builder.Services.AddScoped<CreateUsuarioHandler>();
builder.Services.AddScoped<GetUsuarioByIdHandler>();
builder.Services.AddScoped<ListUsuariosHandler>();
builder.Services.AddScoped<UpdateUsuarioHandler>();
builder.Services.AddScoped<DeleteUsuarioHandler>();

// ---------- ML.NET (Clean Arch) ----------
builder.Services.AddSingleton<ISentimentAnalyzer, SentimentAnalyzerMlNet>();
builder.Services.AddScoped<AnalyzeSentimentHandler>();

var app = builder.Build();

// ---------- Swagger ----------
app.UseSwagger();

// Swagger UI versionado
app.UseSwaggerUI(options =>
{
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    foreach (var desc in provider.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json", $"MotoSecurityX {desc.GroupName.ToUpperInvariant()}");
    }
    options.RoutePrefix = "swagger"; // padrão
});

// ---------- HTTPS opcional ----------
var enableHttpsRedirect = app.Configuration.GetValue<bool>("Https:EnableRedirection", false);
if (enableHttpsRedirect)
    app.UseHttpsRedirection();

// ---------- Health endpoints ----------
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
app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// ---------- Rotas ----------
app.UseMiddleware<ApiKeyMiddleware>();
app.MapControllers();
app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();

// Necessário para WebApplicationFactory em testes de integração
namespace CP4.MotoSecurityX.Api
{
    public partial class Program { }
}
