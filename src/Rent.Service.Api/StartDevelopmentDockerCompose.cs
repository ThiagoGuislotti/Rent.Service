using Ductus.FluentDocker.Services;
using NetToolsKit.Core.Utils;

namespace Rent.Service.Api
{
    public class StartDevelopmentDockerCompose : IHostedService
    {
        #region Variáveis
        private ICompositeService? _compositeService;
        private readonly IHostEnvironment _environment;
        private readonly IHostApplicationLifetime _lifetime;
        private readonly ILogger<StartDevelopmentDockerCompose> _logger;
        #endregion

        #region Construtores
        public StartDevelopmentDockerCompose(
            IHostEnvironment environment,
            IHostApplicationLifetime lifetime,
            ILogger<StartDevelopmentDockerCompose> logger)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _lifetime = lifetime ?? throw new ArgumentNullException(nameof(lifetime));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _compositeService = null;
        }
        #endregion

        #region Métodos/Operadores Públicos
        /// <inheritdoc/>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (_environment.IsDevelopment())
                {
                    _logger.LogInformation("[Starting: DockerCompose]-[{0}] - [{1}]", _environment.EnvironmentName, AppDomain.CurrentDomain.FriendlyName);
                    var currentDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "docker"));
                    _compositeService = new Ductus.FluentDocker.Builders.Builder()
                        .UseContainer()
                        .UseCompose()
                        .ServiceName("rent-services-development")
                        .FromFile(Path.Combine(currentDirectory, "docker-compose-minio.yaml"))
                        .FromFile(Path.Combine(currentDirectory, "docker-compose-postgresql.yaml"))
                        .FromFile(Path.Combine(currentDirectory, "docker-compose-rabbitmq.yaml"))
                        .FromFile(Path.Combine(currentDirectory, "docker-compose-seq.yaml"))
                        .RemoveOrphans()
                        .Build()
                        .Start();

                    _logger.LogInformation("[Started: DockerCompose]-[{0}] - [{1}]", _environment.EnvironmentName, AppDomain.CurrentDomain.FriendlyName);
                    _lifetime.ApplicationStopping.Register(OnShutdown);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("[Error: DockerCompose]-[{0}] - [{1}] Erro: {2}", _environment.EnvironmentName, AppDomain.CurrentDomain.FriendlyName, ex.Message);
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
        #endregion

        #region Métodos/Operadores Privados
        private void OnShutdown()
        {
            try
            {
                if (_compositeService.IsNullOrEmpty() || _compositeService.State != ServiceRunningState.Running)
                    return;

                _logger.LogInformation("[Stopping: DockerCompose]-[{0}] - [{1}]", _environment.EnvironmentName, AppDomain.CurrentDomain.FriendlyName);
                _compositeService.Dispose();
                _logger.LogInformation("[Stopped: DockerCompose]-[{0}] - [{1}]", _environment.EnvironmentName, AppDomain.CurrentDomain.FriendlyName);
            }
            catch (Exception ex)
            {
                _logger.LogError("[Error: DockerCompose]-[{0}] - [{1}] Erro: {2}", _environment.EnvironmentName, AppDomain.CurrentDomain.FriendlyName, ex.Message);
            }
        }
        #endregion
    }
}