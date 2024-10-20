using MediatR;

namespace Rent.Service.Application.Cqrs.Notifications
{
    public sealed class MotorcycleRegisteredNotification : INotification
    {
        #region Propriedades Públicas
        public required Guid TransactionId { get; init; }
        public required string MotorcycleId { get; init; }
        public required short Year { get; init; }
        #endregion
    }
}