using MediatR;

namespace Rent.Service.Application.Services
{
    public interface INotificationService
    {
        #region Métodos/Operadores Públicos   
        Task PublishAsync<TNotification>(TNotification notification)
            where TNotification : INotification;
        #endregion
    }
}