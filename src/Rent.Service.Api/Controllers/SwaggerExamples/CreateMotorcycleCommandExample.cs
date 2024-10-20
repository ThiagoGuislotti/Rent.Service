using Rent.Service.Application.Cqrs.Commands;
using Swashbuckle.AspNetCore.Filters;

namespace Rent.Service.Api.Controllers.SwaggerExamples
{
    public class CreateMotorcycleCommandExample : IMultipleExamplesProvider<CreateMotorcycleCommand>
    {
        #region Métodos/Operadores Públicos
        public IEnumerable<SwaggerExample<CreateMotorcycleCommand>> GetExamples()
        {
            yield return SwaggerExample.Create(
                "Ok",
                new CreateMotorcycleCommand
                {
                    Id = "123456",
                    Model = "Honda CG 160",
                    LicensePlate = "ABC-1234",
                    Year = (short)DateTime.UtcNow.Year,
                });
            yield return SwaggerExample.Create(
                "BadRequest",
                new CreateMotorcycleCommand
                {
                    Id = "xxxxx",
                    Model = "Honda CG 160",
                    LicensePlate = "xxx",
                    Year = (short)DateTime.UtcNow.Year,
                });
        }
        #endregion
    }
}