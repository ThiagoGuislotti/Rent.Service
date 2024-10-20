using NetToolsKit.Core.Extensions.AspNetCore.Formatters;

namespace Rent.Service.Api.Middlewares
{
    public static class MiddlewareExtensions
    {
        #region Métodos/Operadores Públicos
        public static IMvcBuilder AddCustomInputFormatter(
            this IServiceCollection services)
        {
            CustomInputFormatter.OnModelHandled += ModelHandlers.HandleTransactionRecord;

            return services.AddControllers(options => options.InputFormatters.Insert(0, new CustomInputFormatter()));
        }
        #endregion
    }
}
