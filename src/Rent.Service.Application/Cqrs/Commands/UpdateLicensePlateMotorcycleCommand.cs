using MediatR;
using NetToolsKit.Core.Responses;
using Newtonsoft.Json;
using Rent.Service.Application.Cqrs.Helpers;
using Rent.Service.Domain.Helpers;

namespace Rent.Service.Application.Cqrs.Commands
{
    public sealed record UpdateLicensePlateMotorcycleCommand : TransactionRecord, IRequest<IResponseResult>
    {
        #region Variáveis
        private readonly string _licensePlate = default!;
        #endregion

        #region Propriedades Públicas
        [JsonIgnore]
        public string Id { get; init; }  = string.Empty;

        [JsonProperty("placa")]
        public required string LicensePlate
        {
            get => _licensePlate;
            init => _licensePlate = TextSanitizer.RemoveSpecialCharacters(value);
        }
        #endregion
    }
}