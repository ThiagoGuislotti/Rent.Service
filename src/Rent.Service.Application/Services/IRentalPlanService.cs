using Rent.Service.Domain.Enums;

namespace Rent.Service.Application.Services
{
    public interface IRentalPlanService
    {
        #region Métodos/Operadores Públicos   
        int GetDailyValue(RentalPlanType planType);
        int CalculateTotalCost(RentalPlanType planType, DateTimeOffset startDate, DateTimeOffset endDate);
        int CalculatePenalty(RentalPlanType planType, DateTimeOffset expectedEndDate, DateTimeOffset endDate);
        bool CanDeliverMotorcycle(DriverLicenseCategory driverLicenseCategory);
        DateTimeOffset GetExpectedEndDate(RentalPlanType planType, DateTimeOffset startDate);
        #endregion
    }
}