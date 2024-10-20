// See https://aka.ms/new-console-template for more information
using NetToolsKit.Core.Extensions.AspNetCore;
using NetToolsKit.Core.Extensions.AspNetCore.Helpers;
using NetToolsKit.Core.Extensions.Workers;
using Rent.Service.Application.DependencyInjections;
using Rent.Service.Infrastructure.DependencyInjections;
using Serilog;
using Serilog.Exceptions;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using Container = SimpleInjector.Container;

#region ConfigurationBuilder
var configuration = new ConfigurationBuilder()
            .AddNetToolsKitConfiguration()
            .AddNetToolsKitSerilogBeforeBuild();

new GlobalLogContextDetails
{
    CreatedBy = "Thiago Guislotti",
    DescriptionApp = "Sistema de aluguel de motos - Worker",
    LicensedTo = ""
}.AddNetToolsKitPushProperties();
#endregion

try
{
    var host = Host.CreateDefaultBuilder(args)
        .UseSerilog((context, services, configurations) => configurations
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromGlobalLogContext()
            .Enrich.WithExceptionDetails()
            .Enrich.FromLogContext())
        .ConfigureServices((host, services) =>
        {
            #region SimpleInjector
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
            services.UseSimpleInjectorAspNetRequestScoping(container);
            #endregion

            services.AddRentApplication()
                .AddRentInfrastructure(configuration)
                .AddRentMassTransitConsumerInfrastructure(configuration);

            services.AddHostedService<StartAppNotification>();
            services.AddSingleton(configuration);
        })
        .Build();

    await host.RunAsync().ConfigureAwait(false);
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("{0}", "Shut down complete");
    Log.CloseAndFlush();
}