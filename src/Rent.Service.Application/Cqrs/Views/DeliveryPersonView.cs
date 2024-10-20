using Newtonsoft.Json;

namespace Rent.Service.Application.Cqrs.Views
{
    public sealed class DeliveryPersonView
    {
        #region Propriedades Públicas

        [JsonProperty("identificador")]
        public required string Id { get; init; }

        [JsonProperty("nome")]
        public required string Name { get; init; }

        [JsonProperty("cnpj")]
        public required string Cnpj { get; init; }

        [JsonProperty("data_nascimento")]
        public required DateTimeOffset DateOfBirth { get; init; }

        [JsonProperty("numero_cnh")]
        public required string DriverLicenseNumber { get; init; }

        [JsonProperty("tipo_cnh")]
        public required byte DriverLicenseCategory { get; init; }

        [JsonProperty("com_imagem_cnh")]
        public string? WithDriverLicenseImage { get; init; }
        #endregion
    }
}