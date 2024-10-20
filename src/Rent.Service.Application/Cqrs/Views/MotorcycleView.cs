using Newtonsoft.Json;

namespace Rent.Service.Application.Cqrs.Views
{
    public sealed class MotorcycleView
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