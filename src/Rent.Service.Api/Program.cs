using NetToolsKit.Core.Extensions.AspNetCore;
using NetToolsKit.Core.Extensions.AspNetCore.Helpers;
using NetToolsKit.Core.Extensions.Swagger;
using Rent.Service.Api;
using Rent.Service.Api.Middlewares;
using Rent.Service.Application.DependencyInjections;
using Rent.Service.Infrastructure.DependencyInjections;
using System.Reflection;

#region NetToolsKit - ConfigurationBuilder
var configuration = new ConfigurationBuilder()
            .AddNetToolsKitConfiguration()
            .AddNetToolsKitSerilogBeforeBuild();

new GlobalLogContextDetails
{
    CreatedBy = "Thiago Guislotti",
    DescriptionApp = "Sistema de aluguel de motos - Api",
    LicensedTo = ""
}.AddNetToolsKitPushProperties();
#endregion

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(configuration);
builder.Services.AddHostedService<StartDevelopmentDockerCompose>();
builder.Services.AddHostedService<StartDevelopmentServices>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddRentApplication()
    .AddRentInfrastructure(configuration)
    .AddRentMassTransitPublisherInfrastructure(configuration);

#region NetToolsKit - Services
// Add WebApplicationBuilder
builder
    .AddNetToolsKitSerilogAfterBuild()
    .AddNetToolsKitRateLimiterCacheService();

// Add IServiceCollection
builder.Services
    .AddNetToolsKitApplicationModelProvider()
    .AddNetToolsKitCommonInfraServices()
    .AddNetToolsKitLoggerFlags(_ => new LoggerFlags() { RequestFlag = false, ResponseFlag = false })
    .AddNetToolsKitSwaggerGen(
        swaggerApiInfo: new("Sistema de Aluguel de Motos", Assembly.GetExecutingAssembly().GetName().Name),
        entryAssembly: Assembly.GetEntryAssembly()
    );

// Add IMvcBuilder
builder.Services
    .AddNetToolsKitControllersHeaderValidation()
    .AddNetToolsKitNewtonsoftJson();

// Add Middleware
builder.Services.AddCustomInputFormatter();
#endregion

var app = builder.Build();

#region NetToolsKit - Application
app.AddNetToolsKitAdminSafeList()
    .AddNetToolsKitSwagger()
    .UseNetToolsKitLoggerMiddleware()
    .UseMiddleware<CustomExceptionMiddleware>();

#endregion

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
lifetime.ApplicationStarted.Register(() => app.Logger.LogInformation("[Start:WebApi]-[{0}] - [{1}]", app.Environment.EnvironmentName, AppDomain.CurrentDomain.FriendlyName));
lifetime.ApplicationStopping.Register(() => app.Logger.LogInformation("[Stopping:WebApi]-[{0}] - [{1}]", app.Environment.EnvironmentName, AppDomain.CurrentDomain.FriendlyName));
lifetime.ApplicationStopped.Register(() => app.Logger.LogInformation("[Stopped:WebApi]-[{0}] - [{1}]", app.Environment.EnvironmentName, AppDomain.CurrentDomain.FriendlyName));

try
{
    await app.RunAsync().ConfigureAwait(false);
}
catch (Exception ex)
{
    app.Logger.LogError(ex, "Erro ao iniciar a aplicação.");
    throw;
}