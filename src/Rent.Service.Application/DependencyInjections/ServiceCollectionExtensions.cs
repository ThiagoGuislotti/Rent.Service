using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Rent.Service.Application.Behavior;
using System.Reflection;

namespace Rent.Service.Application.DependencyInjections
{
    public static class ServiceCollectionExtensions
    {
        #region Métodos/Operadores Públicos
        public static IServiceCollection AddRentApplication(this IServiceCollection services)
        {
            // AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // MediatR
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            // Validation
            return services
                .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        }
        #endregion
    }
}