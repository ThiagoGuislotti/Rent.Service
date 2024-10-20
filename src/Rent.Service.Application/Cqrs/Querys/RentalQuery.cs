using MediatR;
using NetToolsKit.Core.Domain.Entities;
using Rent.Service.Application.Cqrs.Views;

namespace Rent.Service.Application.Cqrs.Querys
{
    public sealed record RentalQuery : IEntitySearch, IRequest<IEnumerable<RentalView>>
    {
        #region Propriedades Públicas
        public string? Id { get; init; }
        #endregion
    }
}