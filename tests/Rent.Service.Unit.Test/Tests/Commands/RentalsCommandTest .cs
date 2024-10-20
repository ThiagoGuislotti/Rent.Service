using FluentAssertions;
using Rent.Service.Application.Cqrs.Commands;
using Rent.Service.Application.Cqrs.Commands.Validators;
using Xunit.Abstractions;

namespace Rent.Service.Unit.Test.Tests.Commands
{
    [Trait("Rentals", "Command")]
    public class RentalsCommandTest
    {
        #region Variáveis
        private readonly CreateRentalsValidator _createRentalsValidator = new();
        private readonly RentReturnValidator _rentReturnValidator = new();
        private readonly ITestOutputHelper _output;
        #endregion

        #region Construtores
        public RentalsCommandTest(ITestOutputHelper output)
        {
            // Capturing Output - xUnit:
            // https://xunit.net/docs/capturing-output
            _output = output;
        }
        #endregion

        #region Test Create Valid Cases

        [Fact]
        public void CreateRentals_ValidData_ShouldBeValid()
        {
            // Arrange
            var request = new CreateRentalCommand
            {
                DeliveryPersonId = Guid.NewGuid().ToString(),
                MotorcycleId = Guid.NewGuid().ToString(),
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddDays(1),
                ExpectedEndDate = DateTime.UtcNow.AddDays(1),
                Plan = 7
            };

            // Act
            var result = _createRentalsValidator.Validate(request);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        #endregion

        #region Test Create Invalid DeliveryPerson

        [Fact]
        public void CreateRentals_InvalidDeliveryPerson_ShouldBeInvalid()
        {
            // Arrange
            var request = new CreateRentalCommand
            {
                DeliveryPersonId = string.Empty,
                MotorcycleId = Guid.NewGuid().ToString(),
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddDays(1),
                ExpectedEndDate = DateTime.UtcNow.AddDays(1),
                Plan = 7
            };

            // Act
            var result = _createRentalsValidator.Validate(request);
            _output.WriteLine(result.ToString());

            // Assert
            result.IsValid.Should().BeFalse();
        }

        #endregion

        #region Test Create Invalid MotorcycleId

        [Fact]
        public void CreateRentals_InvalidMotorcycleId_ShouldBeInvalid()
        {
            // Arrange
            var request = new CreateRentalCommand
            {
                DeliveryPersonId = Guid.NewGuid().ToString(),
                MotorcycleId = string.Empty,
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddDays(1),
                ExpectedEndDate = DateTime.UtcNow.AddDays(1),
                Plan = 7
            };

            // Act
            var result = _createRentalsValidator.Validate(request);
            _output.WriteLine(result.ToString());

            // Assert
            result.IsValid.Should().BeFalse();
        }

        #endregion

        #region Test Create Invalid StartDate

        [Fact]
        public void CreateRentals_InvalidStartDate_ShouldBeInvalid()
        {
            // Arrange
            var request = new CreateRentalCommand
            {
                DeliveryPersonId = Guid.NewGuid().ToString(),
                MotorcycleId = Guid.NewGuid().ToString(),
                StartDate = DateTime.UtcNow.AddDays(-1),
                EndDate = DateTime.UtcNow.AddDays(1),
                ExpectedEndDate = DateTime.UtcNow.AddDays(1),
                Plan = 7
            };

            // Act
            var result = _createRentalsValidator.Validate(request);
            _output.WriteLine(result.ToString());

            // Assert
            result.IsValid.Should().BeFalse();
        }

        #endregion

        #region Test Create Invalid EndDate

        [Fact]
        public void CreateRentals_InvalidEndDate_ShouldBeInvalid()
        {
            // Arrange
            var request = new CreateRentalCommand
            {
                DeliveryPersonId = Guid.NewGuid().ToString(),
                MotorcycleId = Guid.NewGuid().ToString(),
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddDays(-1),
                ExpectedEndDate = DateTime.UtcNow.AddDays(1),
                Plan = 7
            };

            // Act
            var result = _createRentalsValidator.Validate(request);
            _output.WriteLine(result.ToString());

            // Assert
            result.IsValid.Should().BeFalse();
        }

        #endregion

        #region Test Create Invalid ExpectedEndDate

        [Fact]
        public void CreateRentals_InvalidExpectedEndDate_ShouldBeInvalid()
        {
            // Arrange
            var request = new CreateRentalCommand
            {
                DeliveryPersonId = Guid.NewGuid().ToString(),
                MotorcycleId = Guid.NewGuid().ToString(),
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddDays(1),
                ExpectedEndDate = DateTime.UtcNow.AddDays(-1),
                Plan = 7
            };

            // Act
            var result = _createRentalsValidator.Validate(request);
            _output.WriteLine(result.ToString());

            // Assert
            result.IsValid.Should().BeFalse();
        }

        #endregion

        #region Test Create Invalid Plan

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(200)]
        public void CreateRentals_InvalidPlan_ShouldBeInvalid(byte plan)
        {
            // Arrange
            var request = new CreateRentalCommand
            {
                DeliveryPersonId = Guid.NewGuid().ToString(),
                MotorcycleId = Guid.NewGuid().ToString(),
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddDays(1),
                ExpectedEndDate = DateTime.UtcNow.AddDays(1),
                Plan = plan
            };

            // Act
            var result = _createRentalsValidator.Validate(request);
            _output.WriteLine(result.ToString());

            // Assert
            result.IsValid.Should().BeFalse();
        }

        #endregion

        #region Test Update Valid Cases

        [Fact]
        public void UpdateRentals_ValidData_ShouldBeValid()
        {
            // Arrange
            var request = new RentReturnCommand
            {
                Id = "locacao123",
                ReturnDate = DateTime.UtcNow.AddDays(1)
            };

            // Act
            var result = _rentReturnValidator.Validate(request);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        #endregion

        #region Test Upddate Invalid Id

        [Fact]
        public void UpdateRentals_InvalidRentalId_ShouldBeInvalid()
        {
            // Arrange
            var request = new RentReturnCommand
            {
                Id = string.Empty,
                ReturnDate = DateTime.UtcNow.AddDays(1)
            };

            // Act
            var result = _rentReturnValidator.Validate(request);
            _output.WriteLine(result.ToString());

            // Assert
            result.IsValid.Should().BeFalse();
        }

        #endregion

        #region Test Upddate Invalid ReturnDate

        [Fact]
        public void UpdateRentals_InvalidReturnDate_ShouldBeInvalid()
        {
            // Arrange
            var request = new RentReturnCommand
            {
                Id = "locacao123",
                ReturnDate = DateTime.UtcNow.AddDays(-1)
            };

            // Act
            var result = _rentReturnValidator.Validate(request);
            _output.WriteLine(result.ToString());

            // Assert
            result.IsValid.Should().BeFalse();
        }

        #endregion

        #region Test Upddate Invalid ReturnDate Empty

        [Fact]
        public void UpdateRentals_EmptyReturnDate_ShouldBeInvalid()
        {
            // Arrange
            var request = new RentReturnCommand
            {
                Id = "locacao123",
                ReturnDate = default
            };

            // Act
            var result = _rentReturnValidator.Validate(request);
            _output.WriteLine(result.ToString());

            // Assert
            result.IsValid.Should().BeFalse();
        }

        #endregion
    }
}