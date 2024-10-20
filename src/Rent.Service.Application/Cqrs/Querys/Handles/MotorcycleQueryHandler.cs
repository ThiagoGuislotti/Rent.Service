using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using NetToolsKit.Core.Data.Contracts;
using NetToolsKit.Core.EntityFramework.Repositories;
using Rent.Service.Application.Cqrs.Abstractions;
using Rent.Service.Application.Cqrs.Commands.Handles;
using Rent.Service.Application.Cqrs.Views;
using Rent.Service.Domain.Entities;

namespace Rent.Service.Application.Cqrs.Querys.Handles
{
    public class MotorcycleQueryHandler : HandlerBase<DeliveryPersonCommandHandler>,
        IRequestHandler<MotorcycleQuery, IEnumerable<MotorcycleView>>
    {
        #region Construtores
        public MotorcycleQueryHandler(ILoggerFactory loggerFactory,
            IMapper mapper,
            IUnitOfWork unitOfWork)
            : base(loggerFactory, mapper, unitOfWork)
        {
        }
        #endregion

        #region Métodos/Operadores Públicos
        public async Task<IEnumerable<MotorcycleView>> Handle(MotorcycleQuery query, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return _unitOfWork
                .GetQueryRepository<Motorcycle>()
                .SearchEnumerableAsync(
                    search: query,
                    selector: src => _mapper.Map<MotorcycleView>(src))
                .ToBlockingEnumerable();
        }
        #endregion
    }
}
