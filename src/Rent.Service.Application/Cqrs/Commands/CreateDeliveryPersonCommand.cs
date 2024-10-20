using MediatR;
using NetToolsKit.Core.Responses;
using Newtonsoft.Json;
using Rent.Service.Application.Cqrs.Helpers;
using Rent.Service.Domain.Enums;
using Rent.Service.Domain.Helpers;

namespace Rent.Service.Application.Cqrs.Commands
{
    public sealed record CreateDeliveryPersonCommand : TransactionRecord, IRequest<IResponseResult>
    {
        #region Variáveis
        private readonly string _cnpj = default!;
        #endregion

        #region Propriedades Públicas
        [JsonProperty("identificador")]
        public required string Id { get; init; }

        [JsonProperty("nome")]
        public required string Name { get; init; }

        [JsonProperty("cnpj")]
        public required string Cnpj
        {
            get => _cnpj;
            init => _cnpj = TextSanitizer.RemoveSpecialCharacters(value);
        }

        [JsonProperty("data_nascimento")]
        public required DateTimeOffset DateOfBirth { get; init; }

        [JsonProperty("numero_cnh")]
        public required string DriverLicenseNumber { get; init; }

        [JsonProperty("tipo_cnh")]
        public required DriverLicenseCategory DriverLicenseType { get; init; }

        [JsonProperty("imagem_cnh")]
        public string? DriverLicenseBase64Image { get; init; }
        #endregion
    }
}