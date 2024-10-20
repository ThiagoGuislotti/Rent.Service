using MediatR;
using NetToolsKit.Core.Responses;
using Newtonsoft.Json;
using Rent.Service.Application.Cqrs.Helpers;
using Rent.Service.Domain.Helpers;

namespace Rent.Service.Application.Cqrs.Commands
{
    public sealed record CreateMotorcycleCommand : TransactionRecord, IRequest<IResponseResult>
    {
        #region Variáveis
        private readonly string _licensePlate = default!;
        #endregion

        #region Propriedades Públicas
        [JsonProperty("identificador")]
        public required string Id { get; init; }

        [JsonProperty("ano")]
        public required short Year { get; init; }

        [JsonProperty("modelo")]
        public required string Model { get; init; }

        [JsonProperty("placa")]
        public required string LicensePlate
        {
            get => _licensePlate;
            init => _licensePlate = TextSanitizer.RemoveSpecialCharacters(value);
        }
        #endregion
    }
}