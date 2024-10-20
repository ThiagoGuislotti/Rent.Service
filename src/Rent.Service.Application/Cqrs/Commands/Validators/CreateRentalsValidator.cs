using FluentValidation;
using Rent.Service.Domain.Enums;

namespace Rent.Service.Application.Cqrs.Commands.Validators
{
    public class CreateRentalsValidator : AbstractValidator<CreateRentalCommand>
    {
        public CreateRentalsValidator()
        {
            RuleFor(x => x.DeliveryPersonId).NotEmpty().NotNull();
            RuleFor(x => x.MotorcycleId).NotEmpty().NotNull();
            RuleFor(x => x.StartDate).Must(x => x.Date == DateTime.UtcNow.AddDays(1).Date)
                .WithMessage("O inicio da locação obrigatóriamente é o primeiro dia após a data de criação");
            RuleFor(x => x.EndDate).NotEmpty().GreaterThan(x => x.StartDate);
            RuleFor(x => x.ExpectedEndDate).NotEmpty().GreaterThanOrEqualTo(x => x.StartDate);
            RuleFor(x => x.Plan)
            .NotEmpty()
                .Must(x => Enum.IsDefined(typeof(RentalPlanType), x))
                .WithMessage("Plano invalido.");
        }
    }
}