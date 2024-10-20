namespace Rent.Service.Application.Services
{
    public interface IStorageService
    {
        #region Métodos/Operadores Públicos  
        Task<bool> UploadFileAsync(Stream fileStream, string fileName, string contentType);
        Task<Stream> GetFileAsync(string fileName);
        #endregion
    }
}