using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using NetToolsKit.Core.Data.Contracts;
using NetToolsKit.Core.EntityFramework.Repositories;
using Rent.Service.Application.Cqrs.Abstractions;
using Rent.Service.Application.Cqrs.Commands.Handles;
using Rent.Service.Application.Cqrs.Querys;
using Rent.Service.Application.Cqrs.Views;
using Rent.Service.Domain.Entities;

namespace Rent.Service.Application.Cqrs.Querys.Handles
{
    public class RentalsQueryHandler : HandlerBase<DeliveryPersonCommandHandler>,
        IRequestHandler<RentalQuery, IEnumerable<RentalView>>
    {
        #region Construtores
        public RentalsQueryHandler(ILoggerFactory loggerFactory,
            IMapper mapper,
            IUnitOfWork unitOfWork)
            : base(loggerFactory, mapper, unitOfWork)
        {
        }
        #endregion

        #region Métodos/Operadores Públicos
        public async Task<IEnumerable<RentalView>> Handle(RentalQuery query, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return _unitOfWork
                .GetQueryRepository<Rental>()
                .SearchEnumerableAsync(
                    search: query,
                    selector: src => _mapper.Map<RentalView>(src))
                .ToBlockingEnumerable();
        }
        #endregion
    }
}