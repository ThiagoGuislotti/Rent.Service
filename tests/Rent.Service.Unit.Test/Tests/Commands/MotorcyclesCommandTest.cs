using FluentAssertions;
using Rent.Service.Application.Cqrs.Commands;
using Rent.Service.Application.Cqrs.Commands.Validators;
using Xunit.Abstractions;

namespace Rent.Service.Unit.Test.Tests.Commands
{
    [Trait("Motorcycle", "Command")]
    public class MotorcyclesCommandTest
    {
        #region Variáveis
        private readonly CreateMotorcyclesValidator _createMotorcyclesValidator = new();
        private readonly UpdateLicensePlateMotorcycleValidator _updateLicensePlateMotorcycleValidator = new();
        private readonly ITestOutputHelper _output;
        #endregion

        #region Construtores
        public MotorcyclesCommandTest(ITestOutputHelper output)
        {
            // Capturing Output - xUnit:
            // https://xunit.net/docs/capturing-output
            _output = output;
        }
        #endregion

        #region Test Create Valid Cases

        [Theory]
        [InlineData("123456", 2020, "Honda", "ABC1234")]
        [InlineData("12", 2015, "Yamaha", "XYZ890E")]
        [InlineData("ABC-", 2020, "Honda", "ABC-1234")]
        [InlineData("-564", 2015, "Yamaha", "XYZ-78-0E")]
        public void CreateMotorcycles_ValidData_ShouldBeValid(string id, short year, string model, string licensePlate)
        {
            // Arrange
            var request = new CreateMotorcycleCommand
            {
                Id = id,
                Year = year,
                Model = model,
                LicensePlate = licensePlate
            };

            // Act
            var result = _createMotorcyclesValidator.Validate(request);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        #endregion

        #region Test Create Invalid Id

        [Theory]
        [InlineData("")]
        [InlineData(null)] 
        [InlineData("ANameThatIsWayTooLongToBeValidAndExceedsTheMaximumLimitOfAllowedCharactersForIdentifieraaaaaaaaaaaaaaaaaaaaaaaa")]
        public void CreateMotorcycles_InvalidIdentifier_ShouldBeInvalid(string id)
        {
            // Arrange
            var request = new CreateMotorcycleCommand
            {
                Id = id,
                Year = 2020,
                Model = "Honda",
                LicensePlate = "ABC1234D"
            };

            // Act
            var result = _createMotorcyclesValidator.Validate(request);
            _output.WriteLine(result.ToString());

            // Assert
            result.IsValid.Should().BeFalse();
        }

        #endregion

        #region Test Create Invalid Year

        [Theory]
        [InlineData(1899)] 
        [InlineData(5000)] 
        public void CreateMotorcycles_InvalidYear_ShouldBeInvalid(short year)
        {
            // Arrange
            var request = new CreateMotorcycleCommand
            {
                Id = "ValidIdentifier",
                Year = year,
                Model = "Honda",
                LicensePlate = "ABC1234D"
            };

            // Act
            var result = _createMotorcyclesValidator.Validate(request);
            _output.WriteLine(result.ToString());

            // Assert
            result.IsValid.Should().BeFalse();
        }

        #endregion

        #region Test Create Invalid Model

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("A")] 
        [InlineData("ANameThatIsWayTooLongToBeValidAndExceedsTheMaximumLimitOfAllowedCharactersForModelaaaaaaaaaaaaaaaaaaa")]
        public void CreateMotorcycles_InvalidModel_ShouldBeInvalid(string model)
        {
            // Arrange
            var request = new CreateMotorcycleCommand
            {
                Id = "ValidIdentifier",
                Year = 2020,
                Model = model,
                LicensePlate = "ABC1234D"
            };

            // Act
            var result = _createMotorcyclesValidator.Validate(request);
            _output.WriteLine(result.ToString());

            // Assert
            result.IsValid.Should().BeFalse();
        }

        #endregion

        #region Test Create Invalid LicensePlate

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("123456")] 
        [InlineData("123456789")]
        public void CreateMotorcycles_InvalidVehiclePlate_ShouldBeInvalid(string licensePlate)
        {
            // Arrange
            var request = new CreateMotorcycleCommand
            {
                Id = "ValidIdentifier",
                Year = 2020,
                Model = "Honda",
                LicensePlate = licensePlate
            };

            // Act
            var result = _createMotorcyclesValidator.Validate(request);
            _output.WriteLine(result.ToString());

            // Assert
            result.IsValid.Should().BeFalse();
        }
        #endregion

        #region Test Update Valid Cases

        [Theory]
        [InlineData("ABC123D")]
        [InlineData("XYZ789E")]
        [InlineData("ABC-123D")]
        [InlineData("XYZ---789E")]
        public void UpdateMotorcycles_ValidVehiclePlate_ShouldBeValid(string licensePlate)
        {
            // Arrange
            var request = new UpdateLicensePlateMotorcycleCommand
            {
                Id = "moto123",
                LicensePlate = licensePlate
            };

            // Act
            var result = _updateLicensePlateMotorcycleValidator.Validate(request);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        #endregion

        #region Test Update Invalid LicensePlate

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("123456")]
        [InlineData("123456789")] 
        public void UpdateMotorcycles_InvalidVehiclePlate_ShouldBeInvalid(string licensePlate)
        {
            // Arrange
            var request = new UpdateLicensePlateMotorcycleCommand
            {
                Id = "moto123",
                LicensePlate = licensePlate
            };

            // Act
            var result = _updateLicensePlateMotorcycleValidator.Validate(request);
            _output.WriteLine(result.ToString());

            // Assert
            result.IsValid.Should().BeFalse();
        }

        #endregion
    }
}