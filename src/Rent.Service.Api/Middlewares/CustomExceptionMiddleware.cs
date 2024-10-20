using Microsoft.AspNetCore.Http.Extensions;
using NetToolsKit.Core.Extensions.AspNetCore.Middlewares;
using NetToolsKit.Core.Logs.Helpers;
using NetToolsKit.Core.Transactions;
using NetToolsKit.Core.Utils.Logs.Extensions;
using Newtonsoft.Json;
using Rent.Service.Api.Responses;
using System.Net;

namespace Rent.Service.Api.Middlewares
{
    public class CustomExceptionMiddleware : ExceptionMiddleware
    {
        #region Construtores
        public CustomExceptionMiddleware(RequestDelegate next)
            : base(next)
        {
        }
        #endregion

        #region Métodos/Operadores Protegidos
        /// <inheritdoc />
        protected override async Task HandleExceptionAsync(HttpContext context,
            ILogger<ExceptionMiddleware> logger,
            TransactionData transactionData,
            Exception exception,
            HttpStatusCode httpStatusCode,
            LogLevel logLevel = LogLevel.Warning)
        {
            var eventSource = $"{context.Request.Method}-{context.Request.GetEncodedPathAndQuery()}";
            _ = logger.ApplyLog(logLevel: logLevel, eventId: AppLogEvents.Exception(eventSource), transactionId: transactionData.TransactionId, obj: exception);

            var messageResponse = new MessageResponse()
            {
                Message = exception.Message,
            };
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)httpStatusCode;

            await context.Response.WriteAsync(JsonConvert.SerializeObject(messageResponse)).ConfigureAwait(false);
        }
        #endregion
    }
}