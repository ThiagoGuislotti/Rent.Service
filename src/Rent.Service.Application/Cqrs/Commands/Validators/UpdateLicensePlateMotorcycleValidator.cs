using FluentValidation;

namespace Rent.Service.Application.Cqrs.Commands.Validators
{
    public class UpdateLicensePlateMotorcycleValidator : AbstractValidator<UpdateLicensePlateMotorcycleCommand>
    {
        public UpdateLicensePlateMotorcycleValidator()
        {
            RuleFor(x => x.LicensePlate).NotEmpty().Length(7, 8);
        }
    }
}