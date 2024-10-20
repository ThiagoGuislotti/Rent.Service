using FluentAssertions;
using Rent.Service.Application.Cqrs.Commands;
using Rent.Service.Application.Cqrs.Commands.Validators;
using Rent.Service.Domain.Enums;
using Xunit.Abstractions;

namespace Rent.Service.Unit.Test.Tests.Commands
{
    [Trait("DeliveryPerson", "Command")]
    public class DeliveryPersonCommandTest
    {
        #region Variáveis
        private readonly CreateDeliveryPersonValidator _createDeliveryPersonValidator = new();
        private readonly ITestOutputHelper _output;
        #endregion

        #region Construtores
        public DeliveryPersonCommandTest(ITestOutputHelper output)
        {
            // Capturing Output - xUnit:
            // https://xunit.net/docs/capturing-output
            _output = output;
        }
        #endregion

        #region Test Create Valid Cases

        [Theory]
        [InlineData("22055776000141")]
        [InlineData("61.360.756/0001-36")]
        public void CreateDeliveryPersont_ValidData_ShouldBeValid(string cnpj)
        {
            // Arrange
            var request = new CreateDeliveryPersonCommand
            {
                Id = "ValidIdentifier",
                Name = "Thiago Guislotti",
                Cnpj = cnpj,
                DateOfBirth = DateTime.UtcNow.AddYears(-20),
                DriverLicenseNumber = "71418707884",
                DriverLicenseType = DriverLicenseCategory.B,
                DriverLicenseBase64Image = "Qk1QAAAAAAQAAAAAAABAAAABAAAAAAAAAAEAAQAAAAAAAAA="
            };

            // Act
            var result = _createDeliveryPersonValidator.Validate(request);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        #endregion

        #region Test Create Invalid Identifier

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("ANameThatIsWayTooLongToBeValidAndExceedsTheMaximumAllowedCharacters")]
        public void CreateDeliveryPerson_InvalidIdentifier_ShouldBeInvalid(string identifier)
        {
            // Arrange
            var request = new CreateDeliveryPersonCommand
            {
                Id = identifier,
                Name = "Thiago Guislotti",
                Cnpj = "22055776000141",
                DateOfBirth = DateTime.UtcNow.AddYears(-20),
                DriverLicenseNumber = "1234567",
                DriverLicenseType = DriverLicenseCategory.B,
            };

            // Act
            var result = _createDeliveryPersonValidator.Validate(request);
            _output.WriteLine(result.ToString());

            // Assert
            result.IsValid.Should().BeFalse();
        }

        #endregion

        #region Test Create Invalid Name

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("A")]
        [InlineData("a7B9d3E1f2G5h8I3j6K4l2M8n0O9p3Q7r2S6t5U4v1W9x2Y8z3A7b4C6d5E9f3G7h1I8j4K2l6M0nfdjgnjvfnvdgauafbxfrxv12")]
        public void CreateDeliveryPerson_InvalidName_ShouldBeInvalid(string name)
        {
            // Arrange
            var request = new CreateDeliveryPersonCommand
            {
                Id = "ValidIdentifier",
                Name = name,
                Cnpj = "22055776000141",
                DateOfBirth = DateTime.UtcNow.AddYears(-20),
                DriverLicenseNumber = "1234567",
                DriverLicenseType = DriverLicenseCategory.B,
            };

            // Act
            var result = _createDeliveryPersonValidator.Validate(request);
            _output.WriteLine(result.ToString());

            // Assert
            result.IsValid.Should().BeFalse();
        }

        #endregion

        #region Test Create Invalid Cnpj

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("123")]
        [InlineData("22055776000141567890")]
        [InlineData("12345678abcd90")]
        public void CreateDeliveryPerson_InvalidCnpj_ShouldBeInvalid(string cnpj)
        {
            // Arrange
            var request = new CreateDeliveryPersonCommand
            {
                Id = "ValidIdentifier",
                Name = "Thiago Guislotti",
                Cnpj = cnpj,
                DateOfBirth = DateTime.UtcNow.AddYears(-20),
                DriverLicenseNumber = "1234567",
                DriverLicenseType = DriverLicenseCategory.B,
            };

            // Act
            var result = _createDeliveryPersonValidator.Validate(request);
            _output.WriteLine(result.ToString());

            // Assert
            result.IsValid.Should().BeFalse();
        }

        #endregion

        #region Test Create Invalid DateOfBirth

        [Theory]
        [InlineData(-17)]
        [InlineData(0)]
        [InlineData(18)]
        [InlineData(50)]
        public void CreateDeliveryPerson_InvalidDateOfBirth_ShouldBeInvalid(int dateOfBirth)
        {
            // Arrange
            var request = new CreateDeliveryPersonCommand
            {
                Id = "ValidIdentifier",
                Name = "Thiago Guislotti",
                Cnpj = "22055776000141",
                DateOfBirth = DateTime.UtcNow.AddYears(dateOfBirth),
                DriverLicenseNumber = "1234567",
                DriverLicenseType = DriverLicenseCategory.B,
            };

            // Act
            var result = _createDeliveryPersonValidator.Validate(request);
            _output.WriteLine(result.ToString());

            // Assert
            result.IsValid.Should().BeFalse();
        }

        #endregion

        #region Test Create Invalid DriverLicenseNumber

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("123")]
        [InlineData("220557760001415678901")]
        public void CreateDeliveryPerson_InvalidDriverLicenseNumber_ShouldBeInvalid(string driverLicenseNumber)
        {
            // Arrange
            var request = new CreateDeliveryPersonCommand
            {
                Id = "ValidIdentifier",
                Name = "Thiago Guislotti",
                Cnpj = "22055776000141",
                DateOfBirth = DateTime.UtcNow.AddYears(-20),
                DriverLicenseNumber = driverLicenseNumber,
                DriverLicenseType = DriverLicenseCategory.A,
            };

            // Act
            var result = _createDeliveryPersonValidator.Validate(request);
            _output.WriteLine(result.ToString());

            // Assert
            result.IsValid.Should().BeFalse();
        }

        #endregion

        #region Test Create Invalid DriverLicenseType

        [Theory]
        [InlineData((DriverLicenseCategory)55)]
        public void CreateDeliveryPerson_InvalidDriverLicenseType_ShouldBeInvalid(DriverLicenseCategory driverLicenseType)
        {
            // Arrange
            var request = new CreateDeliveryPersonCommand
            {
                Id = "ValidIdentifier",
                Name = "Thiago Guislotti",
                Cnpj = "22055776000141",
                DateOfBirth = DateTime.UtcNow.AddYears(-20),
                DriverLicenseNumber = "1234567",
                DriverLicenseType = driverLicenseType,
            };

            // Act
            var result = _createDeliveryPersonValidator.Validate(request);
            _output.WriteLine(result.ToString());

            // Assert
            result.IsValid.Should().BeFalse();
        }

        #endregion

        #region Test Create Invalid DriverLicenseImage

        [Theory]
        [InlineData("")]
        [InlineData("invalid_base64_string")]
        public void CreateDeliveryPerson_InvalidDriverLicenseImage_ShouldBeInvalid(string driverLicenseImage)
        {
            // Arrange
            var request = new CreateDeliveryPersonCommand
            {
                Id = "ValidIdentifier",
                Name = "Thiago Guislotti",
                Cnpj = "22055776000141",
                DateOfBirth = DateTime.UtcNow.AddYears(-20),
                DriverLicenseNumber = "1234567",
                DriverLicenseType = DriverLicenseCategory.B,
                DriverLicenseBase64Image = driverLicenseImage
            };

            // Act
            var result = _createDeliveryPersonValidator.Validate(request);
            _output.WriteLine(result.ToString());

            // Assert
            result.IsValid.Should().BeFalse();
        }

        #endregion
    }
}