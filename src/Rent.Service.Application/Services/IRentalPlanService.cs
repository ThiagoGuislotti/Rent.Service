using Rent.Service.Domain.Enums;

namespace Rent.Service.Application.Services
{
    public interface IRentalPlanService
    {
        #region Métodos/Operadores Públicos   
        int GetDailyValue(RentalPlanType planType);
        int CalculateTotalCost(RentalPlanType planType, DateTime startDate, DateTime endDate);
        int CalculatePenalty(RentalPlanType planType, DateTime startDate, DateTime endDate);
        bool CanDeliverMotorcycle(DriverLicenseCategory driverLicenseCategory);
        DateTime GetExpectedEndDate(RentalPlanType planType, DateTime startDate);
        #endregion
    }
}