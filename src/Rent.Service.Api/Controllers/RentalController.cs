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
    public class RentalController : RentServiceControllerBase<RentalController>
    {
        #region Constantes
        private const string ROUTE = "locacao";
        #endregion

        #region Construtores
        public RentalController(
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
        /// Alugar uma moto
        /// </summary>
        /// <returns></returns>
        [HttpPost()]
        [SwaggerOperation(Tags = [ROUTE])]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(MessageResponse))]
        public async Task<ActionResult> PostAsync(
            [FromBody] CreateRentalCommand request,
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken).ConfigureAwait(false);
            return response.Success ? Ok(response.Data) : BadRequest(new MessageResponse() { Message = response.Message });
        }

        /// <summary>
        /// Consultar locações existentes
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        [SwaggerOperation(Tags = [ROUTE])]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RentalView>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(MessageResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(MessageResponse))]
        public async Task<ActionResult<IEnumerable>> GetAsync(
            CancellationToken cancellationToken)
        {
            var query = new RentalQuery();
            var views = await _mediator.Send(query, cancellationToken).ConfigureAwait(false);
            return views.IsNotNullOrEmpty() ? Ok(views) : NotFound(new MessageResponse() { Message = "Nenhuma locação encontrada" });
        }

        /// <summary>
        /// Consultar locação por id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        [SwaggerOperation(Tags = [ROUTE])]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RentalView))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(MessageResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(MessageResponse))]
        public async Task<ActionResult> GetAsync(
            [FromRoute] string id,
            CancellationToken cancellationToken)
        {
            var query = new RentalQuery() { Id = id };
            var views = await _mediator.Send(query, cancellationToken).ConfigureAwait(false);
            var view = views.FirstOrDefault();
            return view.IsNotNullOrEmpty() ? Ok(view) : NotFound(new MessageResponse() { Message = "Locação não encontrada" });
        }

        /// <summary>
        /// Informar data de devolução e calcular valor
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}/devolucao")]
        [SwaggerOperation(Tags = [ROUTE])]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MessageResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(MessageResponse))]
        public async Task<ActionResult> PutAsync(
            [FromBody] RentReturnCommand request,
            [FromRoute] string id,
            CancellationToken cancellationToken)
        {
            request = request with { Id = id };
            var response = await _mediator.Send(request, cancellationToken).ConfigureAwait(false);
            return response.Success ? Ok(new MessageResponse() { Message = $"Valor da locação: R${response.Data}" }) : BadRequest(new MessageResponse() { Message = response.Message });
        }
        #endregion
    }
}