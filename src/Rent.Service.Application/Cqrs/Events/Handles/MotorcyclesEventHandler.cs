using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using NetToolsKit.Core.Data.Contracts;
using NetToolsKit.Core.Logs.Helpers;
using NetToolsKit.Core.Responses;
using NetToolsKit.Core.Utils.Logs.Extensions;
using Rent.Service.Application.Cqrs.Abstractions;
using Rent.Service.Application.Cqrs.Events;
using Rent.Service.Application.Cqrs.Notifications;
using Rent.Service.Domain.Entities;

namespace Rent.Service.Application.Cqrs.Events.Handles
{
    public class MotorcyclesEventHandler : HandlerBase<MotorcyclesEventHandler>,
        INotificationHandler<MotorcycleRegisteredEvent>,
        INotificationHandler<MotorcycleYearRegisteredEvent>
    {
        #region Construtores
        public MotorcyclesEventHandler(ILoggerFactory loggerFactory,
            IMapper mapper,
            IUnitOfWork unitOfWork)
            : base(loggerFactory, mapper, unitOfWork)
        {
        }
        #endregion

        #region Métodos/Operadores Públicos
        public async Task Handle(MotorcycleRegisteredEvent @event, CancellationToken cancellationToken)
        {
            try
            {
                var entity = new MotorcycleEvent()
                {
                    MotorcycleId = @event.MotorcycleId,
                    EventType = nameof(MotorcycleRegisteredEvent)
                };
                var result = await _unitOfWork
                    .GetRepository<MotorcycleEvent>()
                    .InsertAsync(entity, cancellationToken)
                    .ThenDoAsync(() => _unitOfWork.CommitAsync(cancellationToken: cancellationToken))
                    .ConfigureAwait(true);

                _logger.ApplyLog(
                    logLevel: result.Success ? LogLevel.Information : LogLevel.Warning,
                    eventId: AppLogEvents.Exception(nameof(MotorcycleRegisteredNotification)),
                    transactionId: new(@event.TransactionId),
                    obj: result);
            }
            catch (Exception ex)
            {
                _logger.ApplyLogError(
                    eventId: AppLogEvents.Exception(nameof(MotorcycleRegisteredNotification)),
                    transactionId: new(@event.TransactionId),
                    obj: ex);
                throw;
            }
        }

        public async Task Handle(MotorcycleYearRegisteredEvent command, CancellationToken cancellationToken)
        {
            try
            {
                var entity = new MotorcycleEvent()
                {
                    MotorcycleId = command.MotorcycleId,
                    EventType = nameof(MotorcycleYearRegisteredEvent)
                };
                var result = await _unitOfWork
                    .GetRepository<MotorcycleEvent>()
                    .InsertAsync(entity, cancellationToken)
                    .ThenDoAsync(() => _unitOfWork.CommitAsync(cancellationToken: cancellationToken))
                    .ConfigureAwait(true);

                _logger.ApplyLog(
                    logLevel: result.Success ? LogLevel.Information : LogLevel.Warning,
                    eventId: AppLogEvents.Exception(nameof(MotorcycleRegisteredNotification)),
                    transactionId: new(command.TransactionId),
                    obj: result);
            }
            catch (Exception ex)
            {
                _logger.ApplyLogError(
                    eventId: AppLogEvents.Exception(nameof(MotorcycleRegisteredNotification)),
                    transactionId: new(command.TransactionId),
                    obj: ex);
                throw;
            }
        }
        #endregion
    }
}