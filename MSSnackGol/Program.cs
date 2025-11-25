using Azure.Monitor.OpenTelemetry.AspNetCore;
using LibraryAuthentication;
using System.Net;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using LibraryConnection.Context;
using MSSnackGol.Middleware;
using MSSnackGol.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();
builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddDbContext<ApplicationDbContext>();

// Registrar servicios de aplicación
builder.Services.AddSingleton<IQRGeneratorService, QRGeneratorService>();

var environment = builder.Environment.EnvironmentName;
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);

// Configuring CORS service to allow any origin, method, and header for testing purposes
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowEverything", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddCustomJwtAuthentication();
//builder.Services.AddCustomJwtAuthentication();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SnackGol API",
        Version = "v1",
        Description = "API para gestión de productos, carrito y órdenes (Agil-UC MVC)"
    });

    // JWT Bearer in Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Autenticación JWT usando el esquema Bearer. Ejemplo: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

    // XML comments (si está habilitado en csproj)
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

builder.Services.AddHttpClient();
//builder.Services.AddScoped<UserController>();

builder.Services.AddOpenTelemetry().UseAzureMonitor(o =>
{
    var builder = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
    IConfiguration config = builder.Build();

    o.ConnectionString = config["ApplicationInsights:ConnectionString"];

});
builder.Services.AddApplicationInsightsTelemetry();

builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(IPAddress.Any, 5046); // Puerto que est�s usando
});

builder.Services.AddSingleton<JwtTokenHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsProduction() || app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(o =>
    {
        o.SwaggerEndpoint("/swagger/v1/swagger.json", "SnackGol API v1");
        // o.RoutePrefix = string.Empty; // habilita Swagger en la raíz '/'
    });
}

// Solo forzar HTTPS en producción (en dev escuchamos por HTTP:5046)
if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

// Applying CORS policy before authentication and authorization middleware
app.UseCors("AllowEverything");

// Exception handling middleware (centralizado)
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Auto-aplicar migraciones y seed al iniciar la API
try
{
    var useInMemory = (Environment.GetEnvironmentVariable("USE_INMEMORY_DB") ?? "0").Trim() == "1";
    if (!useInMemory)
    {
        using var scope = app.Services.CreateScope();
        var db = new ApplicationDbContext();
        db.Database.Migrate();
    }
    else
    {
        Console.WriteLine("[Startup] Using InMemory database (USE_INMEMORY_DB=1). Skipping migrations.");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"[Startup] Error applying migrations: {ex.Message}");
}

app.MapControllers();

app.MapGet("/dev/throw", () => { throw new InvalidOperationException("Prueba middleware"); });

app.Run();