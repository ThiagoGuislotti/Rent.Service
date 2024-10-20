using MassTransit;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using NetToolsKit.Core.Logs;
using NetToolsKit.Core.Utils;
using Rent.Service.Application.Services;
using Rent.Service.Infrastructure.Configuration;

namespace Rent.Service.Infrastructure.Services
{
    public class MinioStorageService : LoggerHandler<MinioStorageService>, IStorageService
    {
        #region Constantes
        private const string BUCKET_NAME = "cnh-uploads";
        #endregion

        #region Variáveis
        private readonly IMinioClient _minioClient;
        #endregion

        #region Construtores
        public MinioStorageService(ILoggerFactory loggerFactory,
            MinioStorageSettings settings) : base(loggerFactory)
        {
            ArgumentNullException.ThrowIfNull(settings);
            _minioClient = new MinioClient()
                            .WithEndpoint(settings.Endpoint)
                            .WithCredentials(settings.AccessKey, settings.SecretKey)
                            .WithSSL(settings.UseSSL)
                            .Build();
        }
        #endregion

        #region Métodos/Operadores Públicos
        public async Task<bool> UploadFileAsync(Stream fileStream, string fileName, string contentType)
        {
            var bucketExists = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(BUCKET_NAME)).ConfigureAwait(false);
            if (!bucketExists)
                await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(BUCKET_NAME));

            var args = new PutObjectArgs()
                .WithBucket(BUCKET_NAME)
                .WithObject(fileName)
                .WithStreamData(fileStream)
                .WithObjectSize(fileStream.Length)
                .WithContentType(contentType);
            var result = await _minioClient.PutObjectAsync(args).ConfigureAwait(false);
            return result.Etag.IsNotNullOrEmpty();
        }

        public async Task<Stream> GetFileAsync(string fileName)
        {
            var fileStream = new MemoryStream();

            var args = new GetObjectArgs()
                .WithBucket(BUCKET_NAME)
                .WithObject(fileName)
                .WithCallbackStream((stream) => stream.CopyTo(fileStream));
            await _minioClient.GetObjectAsync(args).ConfigureAwait(false);

            fileStream.Seek(0, SeekOrigin.Begin);
            return fileStream;
        }
        #endregion
    }
}