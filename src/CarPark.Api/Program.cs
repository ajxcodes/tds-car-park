using System.Text.Json.Serialization;
using CarPark.Application.Extensions;
using CarPark.Infrastructure.Extensions;

// Create a builder with default settings.
var builder = WebApplication.CreateBuilder(args);

// **Crucially, clear the default configuration sources that enable hot reload.**
builder.Configuration.Sources.Clear();

// Manually add the configuration sources you need, ensuring reloadOnChange is false.
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: false)
    .AddEnvironmentVariables();
var configuration = builder.Configuration;
var services = builder.Services;
services
    .AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
services
    .ConfigureInfrastructure(configuration)
    .ConfigureApplication()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

var app = builder.Build();


app.UseSwagger()
    .UseSwaggerUI();
app.Lifetime.ApplicationStarted.Register(() =>
    Console.WriteLine("--> Swagger UI available on https://localhost:7175/swagger"));


await app.Services.ApplyDatabaseMigrationsAsync();

app.MapControllers();

app.Run();