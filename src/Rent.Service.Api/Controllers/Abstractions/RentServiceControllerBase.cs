using MediatR;
using NetToolsKit.Core.Extensions.AspNetCore.Controllers;
using NetToolsKit.Core.Transactions;

namespace Rent.Service.Api.Controllers.Abstractions
{
    public abstract class RentServiceControllerBase<T> : ControllerBase<T>
    {
        #region Variáveis
        protected readonly IMediator _mediator;
        protected readonly TransactionData _transactionData;
        #endregion

        #region Construtores
        public RentServiceControllerBase(
            IConfiguration configuration,
            IHttpContextAccessor contextAccessor,
            ILoggerFactory loggerFactory,
            IMediator mediator,
            TransactionData transactionData)
            : base(configuration, contextAccessor, loggerFactory)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _transactionData = transactionData ?? throw new ArgumentNullException(nameof(transactionData));
        }
        #endregion
    }
}