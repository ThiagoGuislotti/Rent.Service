using MediatR;
using NetToolsKit.Core.Responses;
using Newtonsoft.Json;
using Rent.Service.Application.Cqrs.Helpers;

namespace Rent.Service.Application.Cqrs.Commands
{
    public sealed record CreateRentalCommand : TransactionRecord, IRequest<IResponseResult<string>>
    {
        #region Variáveis
        private readonly DateTime _startDate;
        #endregion

        #region Propriedades Públicas
        [JsonProperty("identificador")]
        public string? Id { get; init; } = Guid.NewGuid().ToString();

        [JsonProperty("entregdor_id")]
        public required string DeliveryPersonId { get; init; }

        [JsonProperty("moto_id")]
        public required string MotorcycleId { get; init; }

        [JsonProperty("data_inicio")]
        public required DateTime StartDate
        {
            get => _startDate;
            init => _startDate = value.Date;
        }

        [JsonProperty("data_termino")]
        public required DateTime EndDate { get; init; }

        [JsonProperty("data_previsão_termino")]
        public required DateTime ExpectedEndDate { get; init; }

        [JsonProperty("plano")]
        public required byte Plan { get; init; }
        #endregion
    }
}