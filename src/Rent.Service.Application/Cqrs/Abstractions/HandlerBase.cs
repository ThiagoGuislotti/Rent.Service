using AutoMapper;
using Microsoft.Extensions.Logging;
using NetToolsKit.Core.Data.Contracts;
using NetToolsKit.Core.Logs;

namespace Rent.Service.Application.Cqrs.Abstractions
{
    public abstract class HandlerBase<T> : LoggerHandler<T>
    {
        #region Propriedades
        protected readonly IMapper _mapper;
        protected readonly IUnitOfWork _unitOfWork;
        #endregion

        #region Construtores
        public HandlerBase(ILoggerFactory loggerFactory,
            IMapper mapper,
            IUnitOfWork unitOfWork)
            : base(loggerFactory)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }
        #endregion
    }
}