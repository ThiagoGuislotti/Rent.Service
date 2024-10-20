using FluentValidation;

namespace Rent.Service.Application.Cqrs.Commands.Validators
{
    public sealed class CreateMotorcyclesValidator : AbstractValidator<CreateMotorcycleCommand>
    {
        public CreateMotorcyclesValidator()
        {
            RuleFor(x => x.Id).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Year)
                    .NotEmpty()
                    .Must(Year => Year > 1900 && Year <= DateTimeOffset.UtcNow.AddYears(2).Year)
                    .WithMessage("O ano do veículo deve ser válido.");
            RuleFor(x => x.Model).NotEmpty().MaximumLength(100).MinimumLength(2);
            RuleFor(x => x.LicensePlate).NotEmpty().Length(7, 8);
        }
    }
}