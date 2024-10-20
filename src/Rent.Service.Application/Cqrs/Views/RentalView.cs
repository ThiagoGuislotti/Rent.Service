using Newtonsoft.Json;

namespace Rent.Service.Application.Cqrs.Views
{
    public sealed class RentalView
    {
        #region Propriedades Públicas
        [JsonProperty("identificador")]
        public required string Id { get; init; }

        [JsonProperty("entregado_id")]
        public required string DeliveryPersonId { get; init; }

        [JsonProperty("moto_id")]
        public required string MotorcycleId { get; init; }

        [JsonProperty("data_inicio")]
        public required DateTimeOffset StartDate { get; init; }

        [JsonProperty("data_termino")]
        public required DateTimeOffset EndDate { get; init; }

        [JsonProperty("data_previsao_termino")]
        public required DateTimeOffset ExpectedEndDate { get; init; }

        [JsonProperty("plano")]
        public required byte Plan { get; init; }

        [JsonProperty("valor_diaria")]
        public required string DailyValue { get; init; }

        [JsonProperty("data_devolucao")]
        public DateTimeOffset? ReturnDate { get; init; }
        #endregion
    }
}