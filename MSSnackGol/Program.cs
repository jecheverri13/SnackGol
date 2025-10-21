using Azure.Monitor.OpenTelemetry.AspNetCore;
using LibraryAuthentication;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();
builder.Services.AddControllers();
builder.Services.AddHttpClient();


builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

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
builder.Services.AddSwaggerGen();

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
    options.Listen(IPAddress.Any, 5046); // Puerto que estás usando
});

builder.Services.AddSingleton<JwtTokenHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsProduction() || app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Applying CORS policy before authentication and authorization middleware
app.UseCors("AllowEverything");

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();