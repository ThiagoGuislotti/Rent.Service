using Newtonsoft.Json;

namespace Rent.Service.Application.Cqrs.Helpers
{
    public abstract record TransactionRecord
    {
        #region Propriedades Públicas
        [JsonIgnore]
        public Guid TransactionId { get; init; }
        #endregion
    }
}