using MediatR;
using NetToolsKit.Core.Responses;
using Rent.Service.Application.Cqrs.Helpers;

namespace Rent.Service.Application.Cqrs.Commands
{
    public sealed record DeleteMotorcycleCommand : TransactionRecord, IRequest<IResponseResult>
    {
        #region Propriedades Públicas
        public required string Id { get; init; }
        #endregion
    }
}