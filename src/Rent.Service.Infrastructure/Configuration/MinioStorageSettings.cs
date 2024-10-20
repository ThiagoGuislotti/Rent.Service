namespace Rent.Service.Infrastructure.Configuration
{
    public record MinioStorageSettings
    {
        #region Propriedades Públicas
        public required string Endpoint { get; init; }
        public required string AccessKey { get; init; }
        public required string SecretKey { get; init; }
        public bool UseSSL { get; init; } = true;
        #endregion
    }
}