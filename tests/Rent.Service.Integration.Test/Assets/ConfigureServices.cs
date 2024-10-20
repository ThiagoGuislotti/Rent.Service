using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rent.Service.Application.DependencyInjections;
using Rent.Service.Infrastructure.DependencyInjections;

namespace Rent.Service.Integration.Test.Assets
{
    public class ConfigureServices
    {
        #region Propriedades Públicas
        public ServiceProvider ServiceProvider { get; protected set; }
        public IConfigurationRoot Configuration { get; protected set; }
        #endregion

        #region Construtores
        public ConfigureServices()
        {
            Configuration = AddConfiguration();

            var services = new ServiceCollection();

            var logLevel = LogLevel.Information;
            services.AddScoped(sp => LoggerFactory.Create(loggingBuilder =>
            {
                loggingBuilder
                    .ClearProviders()
                    .SetMinimumLevel(logLevel)
                    .AddConsole();
            }));

            services.AddRentApplication()
                .AddRentInfrastructure(Configuration)
                .AddRentMassTransitPublisherInfrastructure(Configuration);

            ServiceProvider = services.BuildServiceProvider();
        }
        #endregion

        #region Métodos/Operadores Privados
        private static IConfigurationRoot AddConfiguration()
        {
            var text = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
                ?? "Notfound";

            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings." + text + ".json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }
        #endregion
    }
}