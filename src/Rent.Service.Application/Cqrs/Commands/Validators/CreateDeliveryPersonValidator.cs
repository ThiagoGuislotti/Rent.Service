using FluentValidation;
using NetToolsKit.Core.Extensions.FluentValidations;
using Rent.Service.Domain.Helpers;

namespace Rent.Service.Application.Cqrs.Commands.Validators
{
    public class CreateDeliveryPersonValidator : AbstractValidator<CreateDeliveryPersonCommand>
    {
        private bool BeAValidAge(DateTimeOffset dateOfBirth)
        {
            int age = DateTimeOffset.UtcNow.Year - dateOfBirth.Year;
            if (dateOfBirth > DateTimeOffset.UtcNow.AddYears(-age)) age--;
            return age >= 18;
        }
        public CreateDeliveryPersonValidator()
        {
            RuleFor(x => x.Id).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Name).NotEmpty().Length(2, 100);
            RuleFor(x => x.Cnpj).NotEmpty().IsValidCNPJ();
            RuleFor(x => x.DateOfBirth).NotEmpty().Must(BeAValidAge);
            RuleFor(x => x.DriverLicenseNumber).NotEmpty().Length(11);
            RuleFor(x => x.DriverLicenseType).IsInEnum();
            RuleFor(x => x.DriverLicenseBase64Image)
                .Must(x => MimeTypeDetector.GetMimeTypeFromBase64(x) is "png" or "bmp")
                .WithMessage("Formato de imagem inválido. Somente imagens PNG ou BMP são permitidas.")
                .When(x => x.DriverLicenseBase64Image != null);
        }
    }
}
