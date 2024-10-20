using MediatR;
using NetToolsKit.Core.Responses;
using Newtonsoft.Json;
using Rent.Service.Application.Cqrs.Helpers;

namespace Rent.Service.Application.Cqrs.Commands
{
    public sealed record RentReturnCommand : TransactionRecord, IRequest<IResponseResult<decimal>>
    {
        #region Propriedades Públicas
        [JsonIgnore]
        public string Id { get; init; } = string.Empty;

        [JsonProperty("data_devolucao")]
        public required DateTime ReturnDate { get; init; }
        #endregion
    }
}