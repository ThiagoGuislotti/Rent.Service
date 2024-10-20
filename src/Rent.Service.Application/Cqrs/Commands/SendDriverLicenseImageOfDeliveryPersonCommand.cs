using MediatR;
using NetToolsKit.Core.Responses;
using Newtonsoft.Json;
using Rent.Service.Application.Cqrs.Helpers;

namespace Rent.Service.Application.Cqrs.Commands
{
    public sealed record SendDriverLicenseImageOfDeliveryPersonCommand : TransactionRecord, IRequest<IResponseResult>
    {
        #region Propriedades Públicas
        [JsonIgnore]
        public string Id { get; init; } = string.Empty;

        [JsonProperty("imagem_cnh")]
        public required string DriverLicenseBase64Image { get; init; }
        #endregion
    }
}
