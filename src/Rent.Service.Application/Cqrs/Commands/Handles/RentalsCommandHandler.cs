using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using NetToolsKit.Core.Data.Contracts;
using NetToolsKit.Core.EntityFramework.Repositories;
using NetToolsKit.Core.Responses;
using NetToolsKit.Core.Utils;
using Rent.Service.Application.Cqrs.Abstractions;
using Rent.Service.Application.Services;
using Rent.Service.Domain.Entities;
using System.Text;

namespace Rent.Service.Application.Cqrs.Commands.Handles
{
    public class RentalsCommandHandler : HandlerBase<RentalsCommandHandler>,
        IRequestHandler<CreateRentalCommand, IResponseResult<string>>,
        IRequestHandler<RentReturnCommand, IResponseResult<decimal>>
    {
        #region Variáveis
        private readonly IRentalPlanService _rentalPlanService;
        #endregion

        #region Construtores
        public RentalsCommandHandler(ILoggerFactory loggerFactory,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IRentalPlanService rentalPlanService)
            : base(loggerFactory, mapper, unitOfWork)
        {
            _rentalPlanService = rentalPlanService ?? throw new ArgumentNullException(nameof(rentalPlanService));
        }
        #endregion

        #region Métodos/Operadores Públicos
        public async Task<IResponseResult<string>> Handle(CreateRentalCommand command, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Rental>(command);
            if (command.EndDate.Date != _rentalPlanService.GetExpectedEndDate(entity.Plan, entity.StartDate).Date)
                return ResponseResult.ErrorResult<string>("Data de final não esta de acordo com o plano.");

            var repository = _unitOfWork.GetRepository<Rental>();
            var stringBuilder = new StringBuilder();

            var licenseCategory = await _unitOfWork
                .GetRepository<DeliveryPerson>()
                .SearchFirstOrDefaultAsync(
                    predicate: src => src.Id == command.DeliveryPersonId,
                    selector: src => new { Type = src.DriverLicenseCategory },
                    cancellationToken: cancellationToken
                ).ConfigureAwait(false);
            if (licenseCategory.IsNullOrEmpty())
                stringBuilder.AppendLine($"Nenhum Entregador encontrado com Id: [{command.DeliveryPersonId}].");
            else if (!_rentalPlanService.CanDeliverMotorcycle(licenseCategory.Type))
                stringBuilder.AppendLine("Entregador não habilitado.");

            var motorcycleExists = await _unitOfWork
                .GetRepository<Motorcycle>()
                .ExistsAsync(src => src.Id == command.MotorcycleId, cancellationToken).ConfigureAwait(false);
            if (!motorcycleExists)
                stringBuilder.AppendLine($"Nenhuma Moto encontrada com Id: [{command.MotorcycleId}].");

            if (stringBuilder.Length > 0)
                return ResponseResult.ErrorResult<string>(stringBuilder.ToString());

            entity = entity with
            {
                Id = Guid.NewGuid().ToString(),
                DailyValue = _rentalPlanService.GetDailyValue(entity.Plan),
            };

            var result = await repository
                .InsertAsync(entity, cancellationToken)
                .ThenDoAsync(() => _unitOfWork.CommitAsync(cancellationToken: cancellationToken))
                .ConfigureAwait(true);

            return result.ReplaceData(entity.Id);
        }

        public async Task<IResponseResult<decimal>> Handle(RentReturnCommand command, CancellationToken cancellationToken)
        {
            var repository = _unitOfWork.GetRepository<Rental>();
            var entity = await repository
                .SearchFirstOrDefaultAsync(
                    querySpecification: new() { ChangeTracker = ChangeTracker.AsNoTracking },
                    predicate: src => src.Id == command.Id,
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            if (entity.IsNullOrEmpty())
                return ResponseResult.ErrorResult<decimal>($"Locação não encontrada para o Id: [{command.Id}].");

            entity = entity with
            {
                ReturnDate = command.ReturnDate,
                UpdatedAt = DateTime.UtcNow
            };

            var result = await repository
                .UpdateAsync(entity, cancellationToken)
                .ThenDoAsync(() => _unitOfWork.CommitAsync(cancellationToken: cancellationToken))
                .ConfigureAwait(true);

            decimal totalCost = _rentalPlanService.CalculateTotalCost(entity.Plan, entity.StartDate, command.ReturnDate) / 100;

            return result.ReplaceData(totalCost);
        }
        #endregion
    }
}