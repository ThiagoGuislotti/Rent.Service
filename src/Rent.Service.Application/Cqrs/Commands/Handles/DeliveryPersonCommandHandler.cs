using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using NetToolsKit.Core.Data.Contracts;
using NetToolsKit.Core.EntityFramework.Repositories;
using NetToolsKit.Core.Logs.Helpers;
using NetToolsKit.Core.Responses;
using NetToolsKit.Core.Utils;
using NetToolsKit.Core.Utils.Logs.Extensions;
using Rent.Service.Application.Cqrs.Abstractions;
using Rent.Service.Application.Cqrs.Notifications;
using Rent.Service.Application.Services;
using Rent.Service.Domain.Entities;
using Rent.Service.Domain.Helpers;
using System.Text;

namespace Rent.Service.Application.Cqrs.Commands.Handles
{
    public class DeliveryPersonCommandHandler : HandlerBase<DeliveryPersonCommandHandler>,
        IRequestHandler<CreateDeliveryPersonCommand, IResponseResult>,
        IRequestHandler<SendDriverLicenseImageOfDeliveryPersonCommand, IResponseResult>
    {
        #region Variáveis
        private readonly IStorageService _storageService;
        #endregion

        #region Construtores
        public DeliveryPersonCommandHandler(ILoggerFactory loggerFactory,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IStorageService storageService)
            : base(loggerFactory, mapper, unitOfWork)
        {
            _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
        }
        #endregion

        #region Métodos/Operadores Públicos
        public async Task<IResponseResult> Handle(CreateDeliveryPersonCommand command, CancellationToken cancellationToken = default)
        {
            var repository = _unitOfWork.GetRepository<DeliveryPerson>();
            var stringBuilder = new StringBuilder();

            var entityExists = await repository.ExistsAsync(src => src.Id == command.Id, cancellationToken).ConfigureAwait(false);
            if (entityExists)
                stringBuilder.AppendLine($"Já existe uma Entregador com Id: [{command.Id}].");

            var cnpjExists = await repository.ExistsAsync(src => src.Cnpj == command.Cnpj, cancellationToken).ConfigureAwait(false);
            if (cnpjExists)
                stringBuilder.AppendLine($"Já existe uma Entregador com CNPJ: [{command.Cnpj}].");

            var cnhExists = await repository.ExistsAsync(src => src.DriverLicenseNumber == command.DriverLicenseNumber, cancellationToken).ConfigureAwait(false);
            if (cnhExists)
                stringBuilder.AppendLine($"Já existe uma Entregador com CNH: [{command.DriverLicenseNumber}].");

            if (stringBuilder.Length > 0)
                return ResponseResult.ErrorResult(stringBuilder.ToString());

            var entity = _mapper.Map<DeliveryPerson>(command);
            if (command.DriverLicenseBase64Image.IsNotNullOrEmpty())
            {
                var mimeType = MimeTypeDetector.GetMimeTypeFromBase64(command.DriverLicenseBase64Image);
                if (mimeType.IsNullOrEmpty())
                    return ResponseResult.ErrorResult("Formato de imagem inválido. Somente imagens PNG ou BMP são permitidas.");

                var byteArray = Encoding.UTF8.GetBytes(command.DriverLicenseBase64Image);
                using Stream stream = new MemoryStream(byteArray);
                var storageResult = await _storageService.UploadFileAsync(stream, entity.Id, mimeType).ConfigureAwait(false);
                if (storageResult)
                    entity = entity with { WithDriverLicenseImage = true };
                else
                    _logger.ApplyLogWarning(
                           eventId: AppLogEvents.Exception(nameof(MotorcycleRegisteredNotification)),
                           transactionId: new(command.TransactionId),
                           additionalInfo: "Erro ao salver imagem no storage.");
            }

            var result = await repository
                .InsertAsync(entity, cancellationToken)
                .ThenDoAsync(() => _unitOfWork.CommitAsync(cancellationToken: cancellationToken))
                .ConfigureAwait(true);

            return result;
        }

        public async Task<IResponseResult> Handle(SendDriverLicenseImageOfDeliveryPersonCommand command, CancellationToken cancellationToken = default)
        {
            var repository = _unitOfWork.GetRepository<DeliveryPerson>();

            var entity = await repository
                .SearchFirstOrDefaultAsync(
                    querySpecification: new() { ChangeTracker = ChangeTracker.AsNoTracking },
                    predicate: src => src.Id == command.Id,
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            if (entity.IsNullOrEmpty())
                return ResponseResult.ErrorResult($"Entregador não encontrada para o Id: [{command.Id}].");

            var mimeType = MimeTypeDetector.GetMimeTypeFromBase64(command.DriverLicenseBase64Image);
            if (mimeType.IsNullOrEmpty())
                return ResponseResult.ErrorResult("Formato de imagem inválido. Somente imagens PNG ou BMP são permitidas.");

            var byteArray = Encoding.UTF8.GetBytes(command.DriverLicenseBase64Image);
            using (Stream stream = new MemoryStream(byteArray))
            {
                var storageResult = await _storageService.UploadFileAsync(stream, entity.Id, mimeType).ConfigureAwait(false);
                if (!storageResult)
                    return ResponseResult.ErrorResult("Erro ao salver imagem no storage.");
            }

            entity = entity with { WithDriverLicenseImage = true, UpdatedAt = DateTime.UtcNow };
            var result = await repository
                .UpdateAsync(entity, cancellationToken)
                .ThenDoAsync(() => _unitOfWork.CommitAsync(cancellationToken: cancellationToken))
                .ConfigureAwait(true);

            return result;
        }
        #endregion
    }
}