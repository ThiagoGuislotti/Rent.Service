using Microsoft.Extensions.Logging;
using NetToolsKit.Core.Logs;
using Rent.Service.Application.Services;
using Rent.Service.Domain.Enums;

namespace Rent.Service.Infrastructure.Services
{
    public class RentalPlanService : LoggerHandler<RentalPlanService>, IRentalPlanService
    {
        #region Contantes
        private const int SevenDaysDailyRate = 3000; // R$30.00, multiplicado por 100
        private const int FifteenDaysDailyRate = 2800; // R$28.00, multiplicado por 100
        private const int ThirtyDaysDailyRate = 2200; // R$22.00, multiplicado por 100
        private const int FortyFiveDaysDailyRate = 2000; // R$20.00, multiplicado por 100
        private const int FiftyDaysDailyRate = 1800; // R$18.00, multiplicado por 100

        private const int SevenDaysPenaltyRate = 20; // 20%
        private const int FifteenDaysPenaltyRate = 40; // 40%
        private const int AdditionalDailyCost = 5000; // R$50.00, multiplicado por 100
        #endregion

        #region Construtores
        public RentalPlanService(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
        }
        #endregion

        #region Métodos/Operadores Públicos
        public int CalculateTotalCost(RentalPlanType planType, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            if (endDate < startDate)
                throw new ArgumentException("A data de término não deve ser inferior à data de início.");

            if (!Enum.IsDefined(typeof(RentalPlanType), planType))
                throw new ArgumentOutOfRangeException(nameof(planType), planType, null);

            var rentalDays = GetUsedDays(startDate, endDate);
            var dailyRate = GetDailyValue(planType);
            var totalCost = dailyRate * rentalDays;

            var expectedEndDate = GetExpectedEndDate(planType, startDate);
            if (endDate < expectedEndDate)
                totalCost += CalculatePenalty(planType, startDate, endDate);
            else if (endDate > expectedEndDate)
                totalCost += AdditionalDailyCost * (endDate - expectedEndDate).Days;

            return totalCost;
        }

        public int CalculatePenalty(RentalPlanType planType, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            var usedDays = GetUsedDays(startDate, endDate);
            var unusedDays = (int)planType - usedDays;
            var penaltyRate = planType switch
            {
                RentalPlanType.SevenDays => SevenDaysPenaltyRate,
                RentalPlanType.FifteenDays => FifteenDaysPenaltyRate,
                _ => 0
            };
            return GetDailyValue(planType) * unusedDays * penaltyRate / 100;
        }

        public bool CanDeliverMotorcycle(DriverLicenseCategory driverLicenseCategory)
            => driverLicenseCategory is DriverLicenseCategory.A or DriverLicenseCategory.AB;

        public int GetDailyValue(RentalPlanType planType)
            => planType switch
            {
                RentalPlanType.SevenDays => SevenDaysDailyRate,
                RentalPlanType.FifteenDays => FifteenDaysDailyRate,
                RentalPlanType.ThirtyDays => ThirtyDaysDailyRate,
                RentalPlanType.FortyFiveDays => FortyFiveDaysDailyRate,
                RentalPlanType.FiftyDays => FiftyDaysDailyRate,
                _ => throw new ArgumentOutOfRangeException(nameof(planType), planType, null)
            };

        public DateTimeOffset GetExpectedEndDate(RentalPlanType planType, DateTimeOffset startDate)
            => startDate.AddDays((int)planType);
        #endregion

        #region Métodos/Operadores Privados
        private static int GetUsedDays(DateTimeOffset startDate, DateTimeOffset endDate)
            => Math.Max((int)Math.Ceiling((endDate - startDate).TotalDays), 1);
        #endregion
    }
}