using FluentValidation;
using Rent.Service.Domain.Helpers;

namespace Rent.Service.Application.Cqrs.Commands.Validators
{
    public class SendDriverLicenseImageOfDeliveryPersonleValidator : AbstractValidator<SendDriverLicenseImageOfDeliveryPersonCommand>
    {
        public SendDriverLicenseImageOfDeliveryPersonleValidator()
        {
            RuleFor(x => x.DriverLicenseBase64Image)
                .Must(x => MimeTypeDetector.GetMimeTypeFromBase64(x) is "png" or "bmp")
                .WithMessage("Formato de imagem inválido. Somente imagens PNG ou BMP são permitidas.");
        }
    }
}