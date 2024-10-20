namespace Rent.Service.Domain.Helpers
{
    public static class MimeTypeDetector
    {
        #region Variáveis
        private static readonly Dictionary<string, string> MimeTypes = new()
        {
            { "89504E47", "png" }, // PNG
            { "424D", "bmp" }      // BMP
        };
        #endregion

        #region Métodos/Operadores Públicos
        public static string? GetMimeTypeFromBase64(string? base64String)
        {
            if (string.IsNullOrWhiteSpace(base64String))
                return default;

            try
            {
                var data = Convert.FromBase64String(base64String);
                var hex = BitConverter.ToString(data).Replace("-", string.Empty).ToUpper();
                foreach (var magicNumber in MimeTypes)
                    if (hex.StartsWith(magicNumber.Key))
                        return magicNumber.Value;
                return default;
            }
            catch
            {
                return default;
            }
        }
        #endregion
    }
}