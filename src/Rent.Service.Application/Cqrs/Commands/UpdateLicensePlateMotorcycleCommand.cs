using MediatR;
using NetToolsKit.Core.Responses;
using Newtonsoft.Json;
using Rent.Service.Application.Cqrs.Helpers;

namespace Rent.Service.Application.Cqrs.Commands
{
    public sealed record UpdateLicensePlateMotorcycleCommand : TransactionRecord, IRequest<IResponseResult>
    {
        #region Propriedades Públicas
        [JsonIgnore]
        public string Id { get; init; }  = string.Empty;

        [JsonProperty("placa")]
        public required string LicensePlate { get; init; }
        #endregion
    }
}