using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NetToolsKit.Core.Logs;
using Newtonsoft.Json;
using NUnit.Framework;
using Rent.Service.Application.Cqrs.Commands;
using Rent.Service.Application.Cqrs.Querys;
using Rent.Service.Integration.Test.Assets;

namespace Rent.Service.Integration.Test.Tests.Commands
{
    [TestFixture]
    [RequiresThread]
    [SetCulture("pt-BR")]
    [Category("Commands")]
    public class MotorcycleHandlerTest
    {
        #region Variáveis
        private ConfigureServices _configureServices;
        private IMediator _mediator;
        #endregion

        #region Métodos SetUp
        [SetUp]
        public void SetUp()
        {
            _configureServices = new ConfigureServices();
            _mediator = _configureServices.ServiceProvider.GetRequiredService<IMediator>();
        }
        #endregion

        #region Métodos Test
        [Test()]
        [Order(1)]
        public async Task Create_Motorcycle_ReturnTrue()
        {
            // Arrange
            var command = new CreateMotorcycleCommand
            {
                Id = "moto123",
                Model = "S2",
                LicensePlate = "ABS1234",
                Year = (short)DateTimeOffset.UtcNow.Year,
            };

            // Act
            var result = await _mediator.Send(command).ConfigureAwait(false);
            LogTest.ConsoleWrite(JsonConvert.SerializeObject(result));

            // Assert
            result.Success.Should().BeTrue();
        }

        [Test()]
        [Order(2)]
        public async Task Update_Motorcycle_ReturnTrue()
        {
            // Arrange
            var command = new UpdateLicensePlateMotorcycleCommand
            {
                Id = "moto123",
                LicensePlate = "ABS-1234",
            };

            // Act
            var result = await _mediator.Send(command).ConfigureAwait(false);
            LogTest.ConsoleWrite(JsonConvert.SerializeObject(result));

            // Assert
            result.Success.Should().BeTrue();
        }

        [Test()]
        [Order(3)]
        public async Task GetById_Motorcycle_ReturnTrue()
        {
            // Arrange
            var command = new MotorcycleQuery
            {
                Id = "moto123",
            };

            // Act
            var result = await _mediator.Send(command).ConfigureAwait(false);
            LogTest.ConsoleWrite(JsonConvert.SerializeObject(result));

            // Assert
            result.Should().HaveCount(1);
        }

        [Test()]
        [Order(4)]
        public async Task Delete_Motorcycle_ReturnTrue()
        {
            // Arrange
            var command = new DeleteMotorcycleCommand
            {
                Id = "moto123",
            };

            // Act
            var result = await _mediator.Send(command).ConfigureAwait(false);
            LogTest.ConsoleWrite(JsonConvert.SerializeObject(result));

            // Assert
            result.Success.Should().BeTrue();
        }

        [Test()]
        [Order(5)]
        public async Task GetAll_Motorcycle_ReturnTrue()
        {
            // Arrange
            var command = new MotorcycleQuery() { };

            // Act
            var result = await _mediator.Send(command).ConfigureAwait(false);
            LogTest.ConsoleWrite(JsonConvert.SerializeObject(result));

            // Assert
            result.Should().HaveCount(0);
        }
        #endregion
    }
}