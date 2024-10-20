using MediatR;
using Rent.Service.Application.Cqrs.Helpers;

namespace Rent.Service.Application.Cqrs.Events
{
    public sealed record MotorcycleRegisteredEvent : TransactionRecord, INotification
    {
        #region Propriedades Públicas
        public required string MotorcycleId { get; init; }
        public DateTimeOffset EventDate { get; } = DateTimeOffset.UtcNow;
        #endregion
    }
}