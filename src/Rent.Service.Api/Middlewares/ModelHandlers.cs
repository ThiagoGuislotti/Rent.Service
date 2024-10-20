using Microsoft.AspNetCore.Mvc.Formatters;
using NetToolsKit.Core.Transactions;
using Rent.Service.Application.Cqrs.Helpers;

namespace Rent.Service.Api.Middlewares
{
    public static class ModelHandlers
    {
        #region Métodos/Operadores Públicos
        public static void HandleTransactionRecord(
            object? model,
            InputFormatterContext context)
        {
            if (model is TransactionRecord record)
            {
                var serviceProvider = context.HttpContext.RequestServices;
                var transactionData = serviceProvider.GetRequiredService<TransactionData>();
                record = record with { TransactionId = transactionData.TransactionId };
            }
        }
        #endregion
    }
}
