using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using NetToolsKit.Core.Data.Contracts;
using NetToolsKit.Core.EntityFramework.Repositories;
using NetToolsKit.Core.Responses;
using NetToolsKit.Core.Utils;
using Rent.Service.Application.Cqrs.Abstractions;
using Rent.Service.Application.Cqrs.Notifications;
using Rent.Service.Application.Services;
using Rent.Service.Domain.Entities;
using System.Text;

namespace Rent.Service.Application.Cqrs.Commands.Handles
{
    public class MotorcyclesCommandHandler : HandlerBase<MotorcyclesCommandHandler>,
        IRequestHandler<CreateMotorcycleCommand, IResponseResult>,
        IRequestHandler<UpdateLicensePlateMotorcycleCommand, IResponseResult>,
        IRequestHandler<DeleteMotorcycleCommand, IResponseResult>
    {
        #region Variáveis
        private readonly INotificationService _notificationService;
        #endregion

        #region Construtores
        public MotorcyclesCommandHandler(ILoggerFactory loggerFactory,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            INotificationService notificationService)
            : base(loggerFactory, mapper, unitOfWork)
        {
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        }
        #endregion

        #region Métodos/Operadores Públicos
        public async Task<IResponseResult> Handle(CreateMotorcycleCommand command, CancellationToken cancellationToken)
        {
            var repository = _unitOfWork.GetRepository<Motorcycle>();
            var stringBuilder = new StringBuilder();

            var entityExists = await repository.ExistsAsync(src => src.Id == command.Id, cancellationToken).ConfigureAwait(false);
            if (entityExists)
                stringBuilder.AppendLine($"Já existe uma Moto com Id: [{command.Id}].");

            var licensePlateExists = await repository.ExistsAsync(src => src.LicensePlate == command.LicensePlate, cancellationToken).ConfigureAwait(false);
            if (licensePlateExists)
                stringBuilder.AppendLine($"Já existe uma Moto com Placa: [{command.LicensePlate}].");

            if (stringBuilder.Length > 0)
                return ResponseResult.ErrorResult(stringBuilder.ToString());

            var entity = _mapper.Map<Motorcycle>(command);
            var result = await repository
                .InsertAsync(entity, cancellationToken)
                .ThenDoAsync(() => _unitOfWork.CommitAsync(cancellationToken: cancellationToken))
                .ConfigureAwait(true);

            var notification = new MotorcycleRegisteredNotification()
            {
                TransactionId = command.TransactionId,
                MotorcycleId = entity.Id,
                Year = entity.Year
            };
            await _notificationService.PublishAsync(notification).ConfigureAwait(false);

            return result;
        }

        public async Task<IResponseResult> Handle(UpdateLicensePlateMotorcycleCommand command, CancellationToken cancellationToken)
        {
            var repository = _unitOfWork.GetRepository<Motorcycle>();
            var entity = await repository
                .SearchFirstOrDefaultAsync(
                    querySpecification: new() { ChangeTracker = ChangeTracker.AsNoTracking },
                    predicate: src => src.Id == command.Id,
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            if (entity.IsNullOrEmpty())
                return ResponseResult.ErrorResult($"Moto não encontrada para o Id: [{command.Id}].");

            entity = entity with { LicensePlate = command.LicensePlate, UpdatedAt = DateTimeOffset.UtcNow };

            var result = await repository
                .UpdateAsync(entity, cancellationToken)
                .ThenDoAsync(() => _unitOfWork.CommitAsync(cancellationToken: cancellationToken))
                .ConfigureAwait(true);

            return result;
        }

        public async Task<IResponseResult> Handle(DeleteMotorcycleCommand command, CancellationToken cancellationToken)
        {
            var repository = _unitOfWork.GetRepository<Motorcycle>();
            var entity = await repository
                .SearchFirstOrDefaultAsync(
                    querySpecification: new() { ChangeTracker = ChangeTracker.AsNoTracking },
                    predicate: src => src.Id == command.Id,
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            if (entity.IsNullOrEmpty())
                return ResponseResult.ErrorResult($"Moto não encontrada para o Id: [{command.Id}].");

            var rentalExists = await _unitOfWork.GetRepository<Rental>().ExistsAsync(src => src.MotorcycleId == command.Id, cancellationToken).ConfigureAwait(false);
            if (rentalExists)
                return ResponseResult.ErrorResult($"Não é possivel excluir pois existe locações para esta moto.");

            var result = await repository
                .DeleteAsync([entity.Id], cancellationToken)
                .ThenDoAsync(() => _unitOfWork.CommitAsync(cancellationToken: cancellationToken))
                .ConfigureAwait(true);

            return result;
        }
        #endregion
    }
}