using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using TubeMonitor.Api.Data;
using TubeMonitor.Api.Exceptions;
using TubeMonitor.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Banco de dados (SQLite via EF Core)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Serviços de domínio
builder.Services.AddScoped<ICanalService, CanalService>();
builder.Services.AddScoped<IVideoService, VideoService>();

builder.Services.AddControllers();

// Tratamento global de erros no padrão ProblemDetails (RFC 9457)
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// Versionamento de endpoints: /api/v1/...
builder.Services
    .AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
    })
    .AddMvc()
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

// Documento OpenAPI da versão 1
builder.Services.AddOpenApi("v1");

var app = builder.Build();

// Aplica as migrations pendentes ao iniciar (cria o arquivo .db automaticamente)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
}

app.UseExceptionHandler();
app.UseStatusCodePages();

app.MapOpenApi();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "TubeMonitor API v1");
    options.RoutePrefix = "swagger";
});

app.MapControllers();

app.Run();
