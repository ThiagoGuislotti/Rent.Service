using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetToolsKit.Core.Cryptography;
using NetToolsKit.Core.EntityFramework;
using NetToolsKit.Core.Extensions.AspNetCore;
using NetToolsKit.Core.Json.Newtonsoft;
using NetToolsKit.Core.Security.HashData;
using Rent.Service.Application.Services;
using Rent.Service.Infrastructure.Configuration;
using Rent.Service.Infrastructure.Context;
using Rent.Service.Infrastructure.Services;
using System.Reflection;

namespace Rent.Service.Infrastructure.DependencyInjections
{
    public static class ServiceCollectionExtensions
    {
        #region Métodos/Operadores Públicos
        public static IServiceCollection AddRentMassTransitConsumerInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // RabbitMQ
            return services.AddMassTransit(registration =>
            {
                registration.AddConsumers(Assembly.GetEntryAssembly());

                registration.SetKebabCaseEndpointNameFormatter();
                registration.UsingRabbitMq((context, cfg) =>
                {
                    cfg.ConfigureNewtonsoftJsonSerializer(options => JsonSettings.RestSerializerSettings);
                    cfg.ConfigureNewtonsoftJsonDeserializer(options => JsonSettings.RestSerializerSettings);
                    cfg.Host(configuration.GetConnectionString("RabbitMq"));
                    cfg.ConfigureEndpoints(context);
                });
            });
        }

        public static IServiceCollection AddRentMassTransitPublisherInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // RabbitMQ
            return services.AddMassTransit(registration =>
            {
                registration.SetKebabCaseEndpointNameFormatter();
                registration.UsingRabbitMq((context, cfg) =>
                {
                    cfg.ConfigureNewtonsoftJsonSerializer(options => JsonSettings.RestSerializerSettings);
                    cfg.ConfigureNewtonsoftJsonDeserializer(options => JsonSettings.RestSerializerSettings);
                    cfg.Host(configuration.GetConnectionString("RabbitMq"));
                    cfg.ConfigureEndpoints(context);
                });
            });
        }

        public static IServiceCollection AddRentInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Hash
            services.AddSingleton<IHashData, HashDataHmacSha256>(x => new HashDataHmacSha256("rent"));

            // TransactionData
            services.AddNetToolsKitTransactionData(ServiceLifetime.Transient);

            // IUnitOfWork
            services.AddNetToolsKitDefaultUnitOfWork<ApplicationDbContext>();

            // Settings
            var settings = configuration.GetOptions<MinioStorageSettings>(nameof(MinioStorageSettings));
            services.AddSingleton(settings);

            // Services
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<IStorageService, MinioStorageService>();
            services.AddTransient<IRentalPlanService, RentalPlanService>();

            // DbContext
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            return services.AddNetToolsKitDbPooling<ApplicationDbContext, GenericDbScopedFactory<ApplicationDbContext>>(
                optionsBuilder => optionsBuilder.UseNpgsql(configuration.GetConnectionString("PostgreSql"),
                    options =>
                    {
                        options.EnableRetryOnFailure(maxRetryCount: 3);
                    })
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors());
        }
        #endregion
    }
}