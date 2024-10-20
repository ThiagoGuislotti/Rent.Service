using MediatR;
using NetToolsKit.Core.Responses;
using Newtonsoft.Json;
using Rent.Service.Application.Cqrs.Helpers;

namespace Rent.Service.Application.Cqrs.Commands
{
    public sealed record CreateRentalCommand : TransactionRecord, IRequest<IResponseResult<string>>
    {
        #region Propriedades Públicas
        [JsonProperty("entregdor_id")]
        public required string DeliveryPersonId { get; init; }

        [JsonProperty("moto_id")]
        public required string MotorcycleId { get; init; }

        [JsonProperty("data_inicio")]
        public required DateTimeOffset StartDate { get; init; }

        [JsonProperty("data_termino")]
        public required DateTimeOffset EndDate { get; init; }

        [JsonProperty("data_previsão_termino")]
        public required DateTimeOffset ExpectedEndDate { get; init; }

        [JsonProperty("plano")]
        public required byte Plan { get; init; }
        #endregion
    }
}