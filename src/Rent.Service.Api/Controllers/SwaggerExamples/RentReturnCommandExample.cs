using Rent.Service.Application.Cqrs.Commands;
using Swashbuckle.AspNetCore.Filters;

namespace Rent.Service.Api.Controllers.SwaggerExamples
{
    public class RentReturnCommandExample : IMultipleExamplesProvider<RentReturnCommand>
    {
        #region Métodos/Operadores Públicos
        public IEnumerable<SwaggerExample<RentReturnCommand>> GetExamples()
        {
            yield return SwaggerExample.Create(
                "Ok",
                new RentReturnCommand
                {
                    ReturnDate = DateTime.UtcNow.AddDays(2).Date,
                });
            yield return SwaggerExample.Create(
                "BadRequest",
                new RentReturnCommand
                {
                    ReturnDate = DateTime.UtcNow.AddDays(-1).Date,
                });
        }
        #endregion
    }
}