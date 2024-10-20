﻿using Rent.Service.Application.Cqrs.Commands;
using Rent.Service.Domain.Enums;
using Swashbuckle.AspNetCore.Filters;

namespace Rent.Service.Api.Controllers.SwaggerExamples
{
    public class CreateDeliveryPersonCommandExample : IMultipleExamplesProvider<CreateDeliveryPersonCommand>
    {
        #region Métodos/Operadores Públicos
        public IEnumerable<SwaggerExample<CreateDeliveryPersonCommand>> GetExamples()
        {
            yield return SwaggerExample.Create(
                "Ok",
                new CreateDeliveryPersonCommand
                {
                    Id = "123456",
                    Name = "Thiago",
                    Cnpj = "29267190000190",
                    DateOfBirth = DateTimeOffset.UtcNow.AddYears(-20),
                    DriverLicenseNumber = "44218110912",
                    DriverLicenseType = DriverLicenseCategory.A,
                }); yield return SwaggerExample.Create(
                "Ok Com CNH",
                new CreateDeliveryPersonCommand
                {
                    Id = "123456",
                    Name = "Thiago",
                    Cnpj = "29267190000190",
                    DateOfBirth = DateTimeOffset.UtcNow.AddYears(-20),
                    DriverLicenseNumber = "44218110912",
                    DriverLicenseType = DriverLicenseCategory.AB,
                });
            yield return SwaggerExample.Create(
                "BadRequest",
                new CreateDeliveryPersonCommand
                {
                    Id = "123456",
                    Name = "Thiago",
                    Cnpj = "xxxxx",
                    DateOfBirth = DateTimeOffset.UtcNow.AddYears(-10),
                    DriverLicenseNumber = "xxxxx",
                    DriverLicenseType = DriverLicenseCategory.A,
                });
        }
        #endregion
    }
}