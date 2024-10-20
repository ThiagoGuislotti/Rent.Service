using MediatR;
using NetToolsKit.Core.Responses;
using Newtonsoft.Json;
using Rent.Service.Application.Cqrs.Helpers;

namespace Rent.Service.Application.Cqrs.Commands
{
    public sealed record CreateMotorcycleCommand : TransactionRecord, IRequest<IResponseResult>
    {
        #region Propriedades Públicas
        [JsonProperty("identificador")]
        public required string Id { get; init; }

        [JsonProperty("ano")]
        public required short Year { get; init; }

        [JsonProperty("modelo")]
        public required string Model { get; init; }

        [JsonProperty("placa")]
        public required string LicensePlate { get; init; }
        #endregion
    }
}