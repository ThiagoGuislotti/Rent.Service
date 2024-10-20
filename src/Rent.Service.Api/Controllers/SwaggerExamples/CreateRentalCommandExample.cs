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
                    Id = "123456",
                    DeliveryPersonId = "123456",
                    MotorcycleId = "123456",
                    StartDate = DateTime.UtcNow.AddDays(1),
                    EndDate = DateTime.UtcNow.AddDays(8).Date,
                    ExpectedEndDate = DateTime.UtcNow.AddDays(8).Date,
                    Plan = 7
                });
            yield return SwaggerExample.Create(
                "BadRequest",
                new CreateRentalCommand
                {
                    Id = "123456",
                    DeliveryPersonId = "123456",
                    MotorcycleId = "123456",
                    StartDate = DateTime.UtcNow.AddDays(1),
                    EndDate = DateTime.UtcNow.AddDays(8).Date,
                    ExpectedEndDate = DateTime.UtcNow.AddDays(8).Date,
                    Plan = 0
                });
        }
        #endregion
    }
}