using Ductus.FluentDocker.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using RabbitMQ.Client;
using Rent.Service.Infrastructure.Configuration;
using Rent.Service.Infrastructure.Context;
using Rent.Service.Integration.Test.Assets;

namespace Rent.Service.Integration.Test.Tests
{
    [SetUpFixture]
    public class GlobalSetup
    {
        #region Variáveis
        private ICompositeService _service;
        #endregion

        #region Métodos OneTimeSetUp
        [OneTimeSetUp]
        public async Task RunBeforeAnyTestsAsync()
        {
            try
            {
                var currentDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "..", "docker"));
                _service = new Ductus.FluentDocker.Builders.Builder()
                    .UseContainer()
                    .UseCompose()
                    .ServiceName("rent-services-development")
                    .FromFile(Path.Combine(currentDirectory, "docker-compose-minio.yaml"))
                    .FromFile(Path.Combine(currentDirectory, "docker-compose-postgresql.yaml"))
                    .FromFile(Path.Combine(currentDirectory, "docker-compose-rabbitmq.yaml"))
                    .RemoveOrphans()
                    .Build()
                    .Start();

                using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
                var cancellationToken = cancellationTokenSource.Token;

                // Espera os serviços subirem
                var dbTask = WaitForDbContextAsync(cancellationToken);
                var minioTask = WaitForMinioAsync(cancellationToken);
                var rabbitTask = WaitForRabbitMqAsync(cancellationToken);

                var completedDbTask = await Task.WhenAny(dbTask, Task.Delay(Timeout.Infinite, cancellationToken)).ConfigureAwait(false);
                var completedMinioTask = await Task.WhenAny(minioTask, Task.Delay(Timeout.Infinite, cancellationToken)).ConfigureAwait(false);
                var completedRabbitTask = await Task.WhenAny(rabbitTask, Task.Delay(Timeout.Infinite, cancellationToken)).ConfigureAwait(false);
 
                if (completedDbTask != dbTask && completedMinioTask != minioTask && completedRabbitTask != rabbitTask)
                    Assert.Ignore("Teste ignorado por falha ao conectar com os serviços.");

                await Task.WhenAll(dbTask, minioTask, rabbitTask);
            }
            catch (Exception ex)
            {
                Assert.Ignore($"Erro ao configurar docker-compose: {ex.Message}");
            }
        }

        [OneTimeTearDown]
        public void RunAfterAllTests()
        {
           _service.Dispose();
        }
        #endregion

        #region Métodos Privados
        private static async Task WaitForDbContextAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var configureServices = new ConfigureServices();
                    var dbContext = configureServices.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    await dbContext.Database.EnsureDeletedAsync(cancellationToken).ConfigureAwait(false);
                    await dbContext.Database.MigrateAsync(cancellationToken).ConfigureAwait(false); 

                    if (await dbContext.Database.CanConnectAsync(cancellationToken))
                        return;
                }
                catch
                {
                }
                await Task.Delay(500, cancellationToken).ConfigureAwait(false);
            }
        }

        private static async Task WaitForMinioAsync(CancellationToken cancellationToken)
        {
            using var httpClient = new HttpClient();
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var configureServices = new ConfigureServices();
                    var settings = configureServices.ServiceProvider.GetRequiredService<MinioStorageSettings>();
                    var response = await httpClient.GetAsync($"http://{settings.Endpoint}/minio/health/live", cancellationToken);
                    if (response.IsSuccessStatusCode)
                        return;
                }
                catch
                {
                }
                await Task.Delay(500, cancellationToken).ConfigureAwait(false);
            }
        }

        private static async Task WaitForRabbitMqAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var configureServices = new ConfigureServices();
                    var factory = new ConnectionFactory()
                    {
                        Uri = new Uri(configureServices.Configuration.GetConnectionString("RabbitMq"))
                    };
                    using var connection = factory.CreateConnection();
                    if (connection.IsOpen)
                        return;
                }
                catch
                {
                }
                await Task.Delay(500, cancellationToken).ConfigureAwait(false);
            }
        }
        #endregion
    }
}