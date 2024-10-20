using MediatR;
using NetToolsKit.Core.Domain.Entities;
using Rent.Service.Application.Cqrs.Views;

namespace Rent.Service.Application.Cqrs.Querys
{
    public sealed record MotorcycleQuery : IEntitySearch, IRequest<IEnumerable<MotorcycleView>>
    {
        #region Propriedades Públicas
        public string? Id { get; init; }
        public string? LicensePlate { get; init; }
        #endregion
    }
}