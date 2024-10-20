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
    public class DeliveryPersonHandlerTest
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
        public async Task Create_DeliveryPerson_ReturnTrue()
        {
            // Arrange
            var command = new CreateDeliveryPersonCommand
            {
                Id = "ValidIdentifier",
                Name = "Test",
                Cnpj = "29267190000190",
                DateOfBirth = DateTimeOffset.UtcNow.AddYears(-20),
                DriverLicenseNumber = "1234567",
                DriverLicenseType = DriverLicenseCategory.B,
                DriverLicenseBase64Image = "Qk1QAAAAAAQAAAAAAABAAAABAAAAAAAAAAEAAQAAAAAAAAA="
            };

            // Act
            var result = await _mediator.Send(command).ConfigureAwait(false);
            Console.WriteLine(JsonConvert.SerializeObject(result));

            // Assert
            result.Success.Should().BeTrue();
        }

        [Test()]
        [Order(2)]
        public async Task GetAll_DeliveryPerson_ReturnTrue()
        {
            // Arrange
            var command = new DeliveryPersonQuery() { };

            // Act
            var result = await _mediator.Send(command).ConfigureAwait(false);
            LogTest.ConsoleWrite(JsonConvert.SerializeObject(result));

            // Assert
            result.Should().HaveCount(1);
        }
        #endregion
    }
}