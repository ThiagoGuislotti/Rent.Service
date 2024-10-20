using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetToolsKit.Core.Transactions;
using NetToolsKit.Core.Utils;
using Rent.Service.Api.Controllers.Abstractions;
using Rent.Service.Api.Controllers.SwaggerExamples;
using Rent.Service.Api.Responses;
using Rent.Service.Application.Cqrs.Commands;
using Rent.Service.Application.Cqrs.Querys;
using Rent.Service.Application.Cqrs.Views;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.Collections;

namespace Rent.Service.Api.Controllers
{

    [ApiController]
    [Route(ROUTE)]
    public class DeliveryPersonController : RentServiceControllerBase<DeliveryPersonController>
    {
        #region Constantes
        private const string ROUTE = "entregadores";
        #endregion

        #region Construtores
        public DeliveryPersonController(
            IConfiguration configuration,
            IHttpContextAccessor contextAccessor,
            ILoggerFactory loggerFactory,
            IMediator mediator,
            TransactionData transactionData)
            : base(configuration, contextAccessor, loggerFactory, mediator, transactionData)
        {
        }
        #endregion

        #region Métodos/Operadores Públicos
        /// <summary>
        /// Cadastrar entregador
        /// </summary>
        /// <returns></returns>
        [HttpPost()]
        [SwaggerOperation(Tags = [ROUTE])]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(MessageResponse))]
        [SwaggerRequestExample(typeof(CreateDeliveryPersonCommand), typeof(CreateDeliveryPersonCommandExample))]
        public async Task<ActionResult> PostAsync(
            [FromBody] CreateDeliveryPersonCommand request,
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken).ConfigureAwait(false);
            return response.Success ? NoContent() : BadRequest(new MessageResponse() { Message = response.Message });
        }

        /// <summary>
        /// Consultar entregadores existentes
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        [SwaggerOperation(Tags = [ROUTE])]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<DeliveryPersonView>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(MessageResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(MessageResponse))]
        public async Task<ActionResult<IEnumerable>> GetAsync(
            CancellationToken cancellationToken)
        {
            var query = new DeliveryPersonQuery();
            var views = await _mediator.Send(query, cancellationToken).ConfigureAwait(false);
            return views.IsNotNullOrEmpty() ? Ok(views) : NotFound(new MessageResponse() { Message = "Nenhuma entregador encontrado" });
        }

        /// <summary>
        /// Enviar foto da CNH
        /// </summary>
        /// <returns></returns>
        [HttpPost("{id}/cnh")]
        [SwaggerOperation(Tags = [ROUTE])]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(MessageResponse))]
        [SwaggerRequestExample(typeof(SendDriverLicenseImageOfDeliveryPersonCommand), typeof(SendDriverLicenseImageOfDeliveryPersonCommandExample))]
        public async Task<ActionResult> PostAsync(
            [FromBody] SendDriverLicenseImageOfDeliveryPersonCommand request,
            [FromRoute] string id,
            CancellationToken cancellationToken)
        {
            request = request with { Id = id };
            var response = await _mediator.Send(request, cancellationToken).ConfigureAwait(false);
            return response.Success 
                ? Ok(new MessageResponse() { Message = "Foto da CNH enviada com sucesso" }) 
                : BadRequest(new MessageResponse() { Message = response.Message });
        }
        #endregion
    }
}