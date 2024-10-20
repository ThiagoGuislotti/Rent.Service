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
        public required DateTime StartDate { get; init; }

        [JsonProperty("data_termino")]
        public required DateTime EndDate { get; init; }

        [JsonProperty("data_previsao_termino")]
        public required DateTime ExpectedEndDate { get; init; }

        [JsonProperty("plano")]
        public required byte Plan { get; init; }

        [JsonProperty("valor_diaria")]
        public required decimal DailyValue { get; init; }

        [JsonProperty("data_devolucao")]
        public DateTime? ReturnDate { get; init; }
        #endregion
    }
}