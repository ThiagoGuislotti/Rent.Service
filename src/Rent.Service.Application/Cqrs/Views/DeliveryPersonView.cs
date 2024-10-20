using Newtonsoft.Json;
using Rent.Service.Domain.Enums;

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
        public required DriverLicenseCategory DriverLicenseCategory { get; init; }

        [JsonProperty("com_imagem_cnh")]
        public bool WithDriverLicenseImage { get; init; }
        #endregion
    }
}