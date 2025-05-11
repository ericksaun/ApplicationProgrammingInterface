using AspNetCoreRateLimit;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Presentation.ApplicationProgrammingInterface.Middleware;

var builder = WebApplication.CreateBuilder(args);

var condiguration = builder.Configuration.GetSection(nameof(Appsettings));

builder.Services.Configure<Appsettings>(condiguration);
// Agregar servicios al contenedor.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API de Challenge NTTDATA",
        Version = "v1",
        Description = "Una API Challenge NTTDATA.",
        Contact = new OpenApiContact
        {
            Name = "Erick Salinas N.",
            Email = "ericksaun@outlook.com"
        }
    });
});


builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("Appsettings:Configurations:Security:IpRateLimiting"));

builder.Services.AddInMemoryRateLimiting();

// Add URL mapping service
// Add application service
// Add exception handler
builder.Services.AddExceptionHandler<GoblalExceptionHandler>();
builder.Services.AddProblemDetails();


var app = builder.Build();
var appsettings = app.Services.GetRequiredService<IOptionsMonitor<Appsettings>>();
var allowOrigins = appsettings.CurrentValue.Configurations.Security.CorsAlows
    .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

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
app.UseAuthorization();
app.MapControllers();

app.Run();
