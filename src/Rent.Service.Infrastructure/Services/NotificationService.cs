using Humanizer;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using NetToolsKit.Core.Logs;
using Rent.Service.Application.Services;

namespace Rent.Service.Infrastructure.Services
{
    public class NotificationService : LoggerHandler<NotificationService>, INotificationService
    {
        #region Variáveis
        private readonly ISendEndpointProvider _sendEndpoint;
        #endregion

        #region Construtores
        public NotificationService(ILoggerFactory loggerFactory,
            ISendEndpointProvider sendEndpoint) : base(loggerFactory)
        {
            _sendEndpoint = sendEndpoint ?? throw new ArgumentNullException(nameof(sendEndpoint));
        }
        #endregion

        #region Métodos/Operadores Públicos
        public async Task PublishAsync<TNotification>(TNotification notification)
            where TNotification : INotification
        {
            ArgumentNullException.ThrowIfNull(notification, nameof(notification));

            var endpoint = await _sendEndpoint.GetSendEndpoint(new Uri($"queue:{notification.GetType().Name.Underscore()}"));
            await endpoint.Send(notification).ConfigureAwait(false);
        }
        #endregion
    }
}