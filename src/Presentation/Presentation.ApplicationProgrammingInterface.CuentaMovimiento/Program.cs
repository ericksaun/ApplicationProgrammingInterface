using AspNetCoreRateLimit;
using Infrastructure.AppProgrammingInt.DataBase.Configuration;
using Infrastructure.AppProgrammingInt.IOC.Dependency;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Azure.Identity;
using Presentation.ApplicationProgrammingInterface.PersonaCliente.Middleware;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Infrastructure.AppProgrammingInt.Refit.Client;

var builder = WebApplication.CreateBuilder(args);

var condiguration = builder.Configuration.GetSection(nameof(Appsettings));

builder.Services.Configure<Appsettings>(condiguration);
// Agregar servicios al contenedor.

ConfigurationIOC.AddDependency(builder.Services);
ConfigureDataBase.AddConfigureDataBase(builder.Services);
RefitConfig.Configure(builder.Services);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API CuentaMovimiento de Challenge NTTDATA",
        Version = "v1",
        Description = "Una API CuentaMovimiento Challenge NTTDATA.",
        Contact = new OpenApiContact
        {
            Name = "Erick Salinas N.",
            Email = "ericksaun@outlook.com"
        }
    });
});
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("Appsettings:Configurations:Security:IpRateLimiting"));

builder.Services.AddInMemoryRateLimiting();

// Add URL mapping service
// Add application service
// Add exception handler
builder.Services.AddExceptionHandler<GoblalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddApplicationInsightsTelemetry(new Microsoft.ApplicationInsights.AspNetCore.Extensions.ApplicationInsightsServiceOptions
{
    ConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]
});


var app = builder.Build();
var appsettings = app.Services.GetRequiredService<IOptionsMonitor<Appsettings>>();
var allowOrigins = appsettings.CurrentValue.Configurations.Security.CorsAlows
    .Split([',', ';'], StringSplitOptions.RemoveEmptyEntries);

// Configurar el pipeline HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/swagger");
        return;
    }
    await next();

});

app.UseHttpsRedirection();
app.UseCors(options =>
{
    options.WithOrigins(allowOrigins);
    options.AllowAnyMethod();
    options.AllowAnyHeader();
    options.AllowCredentials();
});

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
