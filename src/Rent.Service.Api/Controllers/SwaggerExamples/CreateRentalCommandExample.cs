using Rent.Service.Application.Cqrs.Commands;
using Swashbuckle.AspNetCore.Filters;

namespace Rent.Service.Api.Controllers.SwaggerExamples
{
    public class CreateRentalCommandExample : IMultipleExamplesProvider<CreateRentalCommand>
    {
        #region Métodos/Operadores Públicos
        public IEnumerable<SwaggerExample<CreateRentalCommand>> GetExamples()
        {
            yield return SwaggerExample.Create(
                "Ok",
                new CreateRentalCommand
                {
                    DeliveryPersonId = "123456",
                    MotorcycleId = "123456",
                    StartDate = DateTimeOffset.UtcNow.AddDays(1),
                    EndDate = DateTimeOffset.UtcNow.AddDays(8).Date,
                    ExpectedEndDate = DateTimeOffset.UtcNow.AddDays(8).Date,
                    Plan = 7
                });
            yield return SwaggerExample.Create(
                "BadRequest",
                new CreateRentalCommand
                {
                    DeliveryPersonId = "123456",
                    MotorcycleId = "123456",
                    StartDate = DateTimeOffset.UtcNow.AddDays(1),
                    EndDate = DateTimeOffset.UtcNow.AddDays(8).Date,
                    ExpectedEndDate = DateTimeOffset.UtcNow.AddDays(8).Date,
                    Plan = 0
                });
        }
        #endregion
    }
}