using MediatR;
using NetToolsKit.Core.Domain.Entities;
using NetToolsKit.Core.Utils;
using Rent.Service.Application.Cqrs.Views;
using Rent.Service.Domain.Helpers;

namespace Rent.Service.Application.Cqrs.Querys
{
    public sealed record MotorcycleQuery : IEntitySearch, IRequest<IEnumerable<MotorcycleView>>
    {
        #region Variáveis
        private readonly string? _licensePlate;
        #endregion

        #region Propriedades Públicas
        public string? Id { get; init; }
        public string? LicensePlate
        {
            get => _licensePlate;
            init
            {
                _licensePlate = TextSanitizer.RemoveSpecialCharacters(value);
                _licensePlate = _licensePlate.IsNullOrEmpty() ? null : _licensePlate;
            }
        }
        #endregion
    }
}