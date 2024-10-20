using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Rent.Service.Application.Services;
using Rent.Service.Domain.Enums;
using Rent.Service.Integration.Test.Assets;

namespace Rent.Service.Integration.Test.Tests.Services
{
    [TestFixture]
    [RequiresThread]
    [SetCulture("pt-BR")]
    [Category("Services")]
    public class RentalPlanServiceTests
    {
        #region Variáveis
        private ConfigureServices _configureServices;
        private IRentalPlanService _rentalPlanService;
        #endregion

        #region Métodos OneTimeSetUp
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _configureServices = new ConfigureServices();
            _rentalPlanService = _configureServices.ServiceProvider.GetRequiredService<IRentalPlanService>();
        }
        #endregion

        #region Métodos Test
        // Testes com dias de atraso
        [TestCase(RentalPlanType.SevenDays, 2, 37000)]    // 7 dias de plano com 2 dias de atraso
        [TestCase(RentalPlanType.FifteenDays, 3, 65400)]  // 15 dias de plano com 3 dias de atraso
        [TestCase(RentalPlanType.ThirtyDays, 5, 102000)]  // 30 dias de plano com 5 dias de atraso
        [TestCase(RentalPlanType.FortyFiveDays, 10, 160000)] // 45 dias de plano com 10 dias de atraso
        [TestCase(RentalPlanType.FiftyDays, 7, 137600)]   // 50 dias de plano com 7 dias de atraso
        // Testes com dias exatos do plano
        [TestCase(RentalPlanType.SevenDays, 0, 21000)]    // 7 dias de plano sem atraso
        [TestCase(RentalPlanType.FifteenDays, 0, 42000)]  // 15 dias de plano sem atraso
        [TestCase(RentalPlanType.ThirtyDays, 0, 66000)]   // 30 dias de plano sem atraso
        [TestCase(RentalPlanType.FortyFiveDays, 0, 90000)] // 45 dias de plano sem atraso
        [TestCase(RentalPlanType.FiftyDays, 0, 90000)]    // 50 dias de plano sem atraso
        // Testes com dias abaixo do plano
        [TestCase(RentalPlanType.SevenDays, -2, 16200)]   // 5 dias usados no plano de 7 dias
        [TestCase(RentalPlanType.FifteenDays, -5, 33600)] // 10 dias usados no plano de 15 dias
        [TestCase(RentalPlanType.ThirtyDays, -10, 44000)] // 20 dias usados no plano de 30 dias
        [TestCase(RentalPlanType.FortyFiveDays, -15, 60000)] // 30 dias usados no plano de 45 dias
        [TestCase(RentalPlanType.FiftyDays, -20, 54000)]  // 30 dias usados no plano de 50 dias
        public void Should_CalculateTotalRentalCost_WithLateReturn(RentalPlanType planType, int days, int expectedResult)
        {
            // Arrange
            var startDate = DateTimeOffset.UtcNow.AddDays(1);
            var endDate = startDate.AddDays(days + (byte)planType);

            // Act
            var totalCost = _rentalPlanService.CalculateTotalCost(planType, startDate, endDate);

            // Assert
            totalCost.Should().Be(expectedResult);
        }

        [Test]
        public void Should_ThrowException_WhenEndDateIsBeforeStartDate()
        {
            // Arrange
            var planType = RentalPlanType.SevenDays;
            var startDate = DateTimeOffset.Now.AddDays(1);
            var endDate = startDate.AddDays(-1);

            // Act
            Func<int> action = () => _rentalPlanService.CalculateTotalCost(planType, startDate, endDate);

            // Assert
            action.Should().Throw<ArgumentException>().WithMessage("A data de término deve ser posterior à data de início.");
        }

        [TestCase(DriverLicenseCategory.A, true)]
        [TestCase(DriverLicenseCategory.AB, true)]
        [TestCase(DriverLicenseCategory.B, false)]
        public void Should_OnlyAllowCategoryAOrAB_Drivers_ToRentMotorcycle(DriverLicenseCategory category, bool expectedResult)
        {
            // Act
            var result = _rentalPlanService.CanDeliverMotorcycle(category);

            // Assert
            result.Should().Be(expectedResult);
        }
        #endregion
    }
}