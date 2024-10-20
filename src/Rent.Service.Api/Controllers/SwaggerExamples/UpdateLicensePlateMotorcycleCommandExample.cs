using Rent.Service.Application.Cqrs.Commands;
using Swashbuckle.AspNetCore.Filters;

namespace Rent.Service.Api.Controllers.SwaggerExamples
{
    public class UpdateLicensePlateMotorcycleCommandExample : IMultipleExamplesProvider<UpdateLicensePlateMotorcycleCommand>
    {
        #region Métodos/Operadores Públicos
        public IEnumerable<SwaggerExample<UpdateLicensePlateMotorcycleCommand>> GetExamples()
        {
            yield return SwaggerExample.Create(
                "Ok",
                new UpdateLicensePlateMotorcycleCommand
                {
                    LicensePlate = "ABC-4321",
                });
            yield return SwaggerExample.Create(
                "BadRequest",
                new UpdateLicensePlateMotorcycleCommand
                {
                    LicensePlate = "xxx",
                });
        }
        #endregion
    }
}