using NetToolsKit.Core.Utils;
using System.Text.RegularExpressions;

namespace Rent.Service.Domain.Helpers
{
    public static partial class TextSanitizer
    {
        #region Métodos/Operadores Públicos
        public static string RemoveSpecialCharacters(string? input)
            => input.IsNullOrEmpty() ? string.Empty : SanitizeRegex().Replace(input, string.Empty);
        #endregion

        #region Métodos/Operadores Privados
        [GeneratedRegex("[^a-zA-Z0-9]")]
        private static partial Regex SanitizeRegex();
        #endregion
    }
}