using MediatR;
using Rent.Service.Application.Cqrs.Helpers;

namespace Rent.Service.Application.Cqrs.Events
{
    public sealed record MotorcycleYearRegisteredEvent : TransactionRecord, INotification
    {
        #region Propriedades Públicas
        public required string MotorcycleId { get; init; }
        public DateTime EventDate { get; } = DateTime.UtcNow;
        #endregion
    }
}