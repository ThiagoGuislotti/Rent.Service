using FluentValidation;

namespace Rent.Service.Application.Cqrs.Commands.Validators
{
    public class RentReturnValidator : AbstractValidator<RentReturnCommand>
    {
        public RentReturnValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.ReturnDate).NotEmpty().GreaterThan(DateTime.UtcNow);
        }
    }
}