using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NetToolsKit.Core.Logs;
using Newtonsoft.Json;
using NUnit.Framework;
using Rent.Service.Application.Cqrs.Commands;
using Rent.Service.Application.Cqrs.Querys;
using Rent.Service.Domain.Enums;
using Rent.Service.Integration.Test.Assets;

namespace Rent.Service.Integration.Test.Tests.Commands
{
    [TestFixture]
    [RequiresThread]
    [SetCulture("pt-BR")]
    [Category("Commands")]
    public class RentalHandlerTest
    {
        #region Variáveis
        private ConfigureServices _configureServices;
        private IMediator _mediator;

        private readonly string _deliveryPersonId = Guid.NewGuid().ToString();
        private readonly string _motorcycleId = Guid.NewGuid().ToString();
        private string _rentalId;
        #endregion

        #region Métodos SetUp
        [SetUp]
        public void SetUp()
        {
            _configureServices = new ConfigureServices();
            _mediator = _configureServices.ServiceProvider.GetRequiredService<IMediator>();
        }
        #endregion

        #region Métodos OneTimeSetUp
        [OneTimeSetUp]
        public async Task OneTimeSetUpAsync()
        {
            _configureServices = new ConfigureServices();
            _mediator = _configureServices.ServiceProvider.GetRequiredService<IMediator>();
            var deliveryPersonCommand = new CreateDeliveryPersonCommand
            {
                Id = _deliveryPersonId,
                Name = "Test",
                Cnpj = "06724307000111",
                DateOfBirth = DateTime.UtcNow.AddYears(-20),
                DriverLicenseNumber = "71418707894",
                DriverLicenseType = DriverLicenseCategory.AB,
            };
            var motorcycleCommand = new CreateMotorcycleCommand
            {
                Id = _motorcycleId,
                Model = "S2",
                LicensePlate = "ABS-1245",
                Year = 2024,
            };
            var result = await _mediator.Send(deliveryPersonCommand).ConfigureAwait(false);
            if (!result.Success)
                throw new Exception(result.Message);
            result = await _mediator.Send(motorcycleCommand).ConfigureAwait(false);
            if (!result.Success)
                throw new Exception(result.Message);
        }
        #endregion

        #region Métodos Test
        [Test()]
        [Order(1)]
        public async Task Create_Rental_ReturnTrue()
        {
            // Arrange
            var command = new CreateRentalCommand
            {
                DeliveryPersonId = _deliveryPersonId,
                MotorcycleId = _motorcycleId,
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddDays(8),
                ExpectedEndDate = DateTime.UtcNow.AddDays(8),
                Plan = 7
            };

            // Act
            var result = await _mediator.Send(command).ConfigureAwait(false);
            LogTest.ConsoleWrite(JsonConvert.SerializeObject(result));

            // Assert
            result.Success.Should().BeTrue();
            _rentalId = result.Data;
        }

        [Test()]
        [Order(2)]
        public async Task Update_Rental_ReturnTrue()
        {
            // Arrange
            var command = new RentReturnCommand
            {
                Id = _rentalId,
                ReturnDate = DateTime.UtcNow.AddDays(1)
            };

            // Act
            var result = await _mediator.Send(command).ConfigureAwait(false);
            LogTest.ConsoleWrite(JsonConvert.SerializeObject(result));

            // Assert
            result.Success.Should().BeTrue();
        }

        [Test()]
        [Order(3)]
        public async Task GetById_Rental_ReturnTrue()
        {
            // Arrange
            var command = new RentalQuery
            {
                Id = _rentalId,
            };

            // Act
            var result = await _mediator.Send(command).ConfigureAwait(false);
            LogTest.ConsoleWrite(JsonConvert.SerializeObject(result));

            // Assert
            result.Should().HaveCount(1);
        }

        [Test()]
        [Order(4)]
        public async Task GetAll_Rental_ReturnTrue()
        {
            // Arrange
            var command = new RentalQuery() { };

            // Act
            var result = await _mediator.Send(command).ConfigureAwait(false);
            LogTest.ConsoleWrite(JsonConvert.SerializeObject(result));

            // Assert
            result.Should().HaveCount(1);
        }

        [Test()]
        [Order(5)]
        public async Task CreateRental_Plan50_ReturnTrue()
        {
            // Arrange
            var command = new CreateRentalCommand
            {
                DeliveryPersonId = _deliveryPersonId,
                MotorcycleId = _motorcycleId,
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddDays(51),
                ExpectedEndDate = DateTime.UtcNow.AddDays(51),
                Plan = 50
            };

            // Act
            var result = await _mediator.Send(command).ConfigureAwait(false);
            LogTest.ConsoleWrite(JsonConvert.SerializeObject(result));

            // Assert
            result.Success.Should().BeTrue();
            _rentalId = result.Data;
        }
        #endregion
    }
}