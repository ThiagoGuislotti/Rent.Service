using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using Rent.Service.Infrastructure.Configuration;
using Rent.Service.Infrastructure.Context;

namespace Rent.Service.Api
{
    public class StartDevelopmentServices : IHostedService
    {
        #region Variáveis
        private readonly IHostEnvironment _environment;
        private readonly ILogger<StartDevelopmentServices> _logger;
        private readonly IServiceProvider _serviceProvider;
        #endregion

        #region Construtores
        public StartDevelopmentServices(
            IHostEnvironment environment,
            ILogger<StartDevelopmentServices> logger,
            IServiceProvider serviceProvider)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }
        #endregion

        #region Métodos/Operadores Públicos
        /// <inheritdoc/>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (!_environment.IsProduction())
                {
                    _logger.LogInformation("[Starting: Services]-[{0}] - [{1}]", _environment.EnvironmentName, AppDomain.CurrentDomain.FriendlyName);
 
                    using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
                    var token = cancellationTokenSource.Token;

                    // Espera os serviços subirem
                    var dbTask = WaitForDbContextAsync(token);
                    var minioTask = WaitForMinioAsync(token);
                    var rabbitTask = WaitForRabbitMqAsync(token);

                    var completedDbTask = await Task.WhenAny(dbTask, Task.Delay(Timeout.Infinite, token)).ConfigureAwait(false);
                    var completedMinioTask = await Task.WhenAny(minioTask, Task.Delay(Timeout.Infinite, token)).ConfigureAwait(false);
                    var completedRabbitTask = await Task.WhenAny(rabbitTask, Task.Delay(Timeout.Infinite, token)).ConfigureAwait(false);

                    await Task.WhenAll(dbTask, minioTask, rabbitTask);

                    _logger.LogInformation("[Started: Services]-[{0}] - [{1}]", _environment.EnvironmentName, AppDomain.CurrentDomain.FriendlyName);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("[Error: Services]-[{0}] - [{1}] Message: {2}", _environment.EnvironmentName, AppDomain.CurrentDomain.FriendlyName, ex.Message);
            }
        }

        /// <inheritdoc/>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
        #endregion

        #region Métodos/Operadores Privados
        private async Task WaitForDbContextAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var dbContext = _serviceProvider.GetRequiredService<ApplicationDbContext>();
                    await dbContext.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);

                    if (await dbContext.Database.CanConnectAsync(cancellationToken))
                        return;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("[Warning: Start DbContext]-[{0}] - [{1}] Message: {2}", _environment.EnvironmentName, AppDomain.CurrentDomain.FriendlyName, ex.Message);
                }
                await Task.Delay(2000, cancellationToken).ConfigureAwait(false);
            }
        }

        private async Task WaitForMinioAsync(CancellationToken cancellationToken)
        {
            using var httpClient = new HttpClient();
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var settings = _serviceProvider.GetRequiredService<MinioStorageSettings>();
                    var response = await httpClient.GetAsync($"http://{settings.Endpoint}/minio/health/live", cancellationToken);
                    if (response.IsSuccessStatusCode)
                        return;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("[Warning: Start Minio]-[{0}] - [{1}] Message: {2}", _environment.EnvironmentName, AppDomain.CurrentDomain.FriendlyName, ex.Message);
                }
                await Task.Delay(2000, cancellationToken).ConfigureAwait(false);
            }
        }

        private async Task WaitForRabbitMqAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var configuration = _serviceProvider.GetRequiredService<IConfigurationRoot>();
                    var factory = new ConnectionFactory()
                    {
                        Uri = new Uri(configuration.GetConnectionString("RabbitMq") ?? throw new ArgumentNullException("ConnectionString__RabbitMq"))
                    };
                    using var connection = factory.CreateConnection();
                    if (connection.IsOpen)
                        return;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("[Warning: Start RabbitMq]-[{0}] - [{1}] Message: {2}", _environment.EnvironmentName, AppDomain.CurrentDomain.FriendlyName, ex.Message);
                }
                await Task.Delay(3000, cancellationToken).ConfigureAwait(false);
            }
        }
        #endregion
    }
}