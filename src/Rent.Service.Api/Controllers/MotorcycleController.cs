using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetToolsKit.Core.Transactions;
using NetToolsKit.Core.Utils;
using Rent.Service.Api.Controllers.Abstractions;
using Rent.Service.Api.Responses;
using Rent.Service.Application.Cqrs.Commands;
using Rent.Service.Application.Cqrs.Querys;
using Rent.Service.Application.Cqrs.Views;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections;

namespace Rent.Service.Api.Controllers
{
    [ApiController]
    [Route(ROUTE)]
    public class MotorcycleController : RentServiceControllerBase<MotorcycleController>
    {
        #region Constantes
        private const string ROUTE = "motos";
        #endregion

        #region Construtores
        public MotorcycleController(
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
        /// Cadastrar uma nova moto
        /// </summary>
        /// <returns></returns>
        [HttpPost()]
        [SwaggerOperation(Tags = [ROUTE])]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(MessageResponse))]
        public async Task<ActionResult> PostAsync(
            [FromBody] CreateMotorcycleCommand request,
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken).ConfigureAwait(false);
            return response.Success ? NoContent() : BadRequest(new MessageResponse() { Message = response.Message });
        }

        /// <summary>
        /// Consultar motos existentes
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        [SwaggerOperation(Tags = [ROUTE])]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MotorcycleView>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(MessageResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(MessageResponse))]
        public async Task<ActionResult<IEnumerable>> GetAsync(
            [FromQuery] string? placa,
            CancellationToken cancellationToken)
        {
            var query = new MotorcycleQuery() { LicensePlate = placa };
            var views = await _mediator.Send(query, cancellationToken).ConfigureAwait(false);
            return views.IsNotNullOrEmpty() ? Ok(views) : NotFound(new MessageResponse() { Message = "Nenhuma moto encontrada" });
        }

        /// <summary>
        /// Consultar motos existentes por id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        [SwaggerOperation(Tags = [ROUTE])]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MotorcycleView))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(MessageResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(MessageResponse))]
        public async Task<ActionResult> GetByIdAsync(
            [FromRoute] string id,
            CancellationToken cancellationToken)
        {
            var query = new MotorcycleQuery() { Id = id };
            var views = await _mediator.Send(query, cancellationToken).ConfigureAwait(false);
            var view = views.FirstOrDefault();
            return view.IsNotNullOrEmpty() ? Ok(view) : NotFound(new MessageResponse() { Message = "Moto não encontrada" });
        }

        /// <summary>
        /// Modificar a placa de uma moto
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}/placa")]
        [SwaggerOperation(Tags = [ROUTE])]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MessageResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(MessageResponse))]
        public async Task<ActionResult> PutAsync(
            [FromBody] UpdateLicensePlateMotorcycleCommand request,
            [FromRoute] string id,
            CancellationToken cancellationToken)
        {
            request = request with { Id = id };
            var response = await _mediator.Send(request, cancellationToken).ConfigureAwait(false);
            return response.Success ? Ok(new MessageResponse() { Message = "Placa modificada com sucesso" }) : BadRequest(new MessageResponse() { Message = response.Message });
        }

        /// <summary>
        /// Remover uma moto
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [SwaggerOperation(Tags = [ROUTE])]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(MessageResponse))]
        public async Task<ActionResult> DeleteAsync(
            [FromRoute] string id,
            CancellationToken cancellationToken)
        {
            var request = new DeleteMotorcycleCommand() { Id = id };
            var response = await _mediator.Send(request, cancellationToken).ConfigureAwait(false);
            return response.Success ? Ok(new MessageResponse() { Message = "Moto removida com sucesso" }) : BadRequest(new MessageResponse() { Message = response.Message });
        }
        #endregion
    }
}