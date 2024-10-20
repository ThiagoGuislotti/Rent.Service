using Humanizer;
using MassTransit;
using MediatR;
using NetToolsKit.Core.Logs;
using NetToolsKit.Core.Logs.Helpers;
using NetToolsKit.Core.Transactions;
using NetToolsKit.Core.Utils.Logs.Extensions;
using Rent.Service.Application.Cqrs.Events;
using Rent.Service.Application.Cqrs.Notifications;

namespace Rent.Service.Worker.Consumers
{
    public class MotorcycleNotificationConsumer : LoggerHandler<MotorcycleNotificationConsumer>, 
        IConsumer<MotorcycleRegisteredNotification>
    {
        #region Variáveis
        protected readonly IMediator _mediator;
        #endregion

        #region Construtores
        public MotorcycleNotificationConsumer(
            ILoggerFactory loggerFactory,
            IMediator mediator) : base(loggerFactory)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        #endregion

        #region Métodos/Operadores Públicos
        public async Task Consume(ConsumeContext<MotorcycleRegisteredNotification> context)
        {
            var notification = context.Message ;
            var transactionId = new TransactionId(notification.TransactionId);
            try
            {
                INotification motorcycleEvent = notification.Year == 2024 
                    ? new MotorcycleYearRegisteredEvent() { MotorcycleId = notification.MotorcycleId }
                    : new MotorcycleRegisteredEvent() { MotorcycleId = notification.MotorcycleId };
                await _mediator.Publish(motorcycleEvent).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.ApplyLogError(
                    eventId: AppLogEvents.Exception(nameof(MotorcycleRegisteredNotification)),
                    transactionId: transactionId,
                    obj: ex); 
                throw;
            }
        }
        #endregion
    }

    public class MotorcycleNotificationrDefinition : ConsumerDefinition<MotorcycleNotificationConsumer>
    {
        #region Construtores
        public MotorcycleNotificationrDefinition()
        {
            Endpoint(c =>
            {
                c.ConfigureConsumeTopology = false;
                c.Name = nameof(MotorcycleRegisteredNotification).Underscore();
            });
        }
        #endregion

        #region Métodos/Operadores Públicos
        protected override void ConfigureConsumer(
            IReceiveEndpointConfigurator endpointConfigurator,
            IConsumerConfigurator<MotorcycleNotificationConsumer> consumerConfigurator,
            IRegistrationContext context)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
            endpointConfigurator.UseInMemoryOutbox(context);
        }
        #endregion
    }
}