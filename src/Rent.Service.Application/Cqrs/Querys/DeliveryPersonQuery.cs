using MediatR;
using NetToolsKit.Core.Domain.Entities;
using Rent.Service.Application.Cqrs.Views;

namespace Rent.Service.Application.Cqrs.Querys
{
    public sealed record DeliveryPersonQuery : IEntitySearch, IRequest<IEnumerable<DeliveryPersonView>>
    {
        #region Propriedades Públicas
        public string? Name { get; init; }
        public string? Cnpj { get; init; }
        #endregion
    }
}