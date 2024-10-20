using FluentValidation;
using MediatR;

namespace Rent.Service.Application.Behavior
{
    public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : class
    {
        #region Variáveis
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        #endregion

        #region Construtores
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators ?? [];
        }
        #endregion

        #region Métodos/Operadores Públicos
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                context = new ValidationContext<TRequest>(request);

                var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

                if (failures.Count != 0)
                {
                    var errors = failures.Select(x => $"{Environment.NewLine} -- {x.PropertyName}: {x.ErrorMessage} Gravidade: {x.Severity}");
                    throw new NetToolsKit.Core.Exceptions.ValidationException("Falha de validação: " + string.Join(string.Empty, errors));
                }
            }

            return await next();
        }
        #endregion
    }
}