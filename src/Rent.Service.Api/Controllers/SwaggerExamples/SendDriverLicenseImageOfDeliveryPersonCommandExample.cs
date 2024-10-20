﻿using Rent.Service.Application.Cqrs.Commands;
using Swashbuckle.AspNetCore.Filters;

namespace Rent.Service.Api.Controllers.SwaggerExamples
{
    public class SendDriverLicenseImageOfDeliveryPersonCommandExample : IMultipleExamplesProvider<SendDriverLicenseImageOfDeliveryPersonCommand>
    {
        #region Métodos/Operadores Públicos
        public IEnumerable<SwaggerExample<SendDriverLicenseImageOfDeliveryPersonCommand>> GetExamples()
        {
            yield return SwaggerExample.Create(
                "Ok",
                new SendDriverLicenseImageOfDeliveryPersonCommand
                {
                });
            yield return SwaggerExample.Create(
                "BadRequest",
                new SendDriverLicenseImageOfDeliveryPersonCommand
                {
                    DriverLicenseBase64Image = "123456"
                });
        }
        #endregion
    }
}