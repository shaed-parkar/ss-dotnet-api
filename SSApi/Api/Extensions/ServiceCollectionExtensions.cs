using Microsoft.ApplicationInsights.Extensibility;
using SS.Common;
using SS.DAL.Commands.Core;
using SS.DAL.Queries.Core;

namespace Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomTypes(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ILoggingDataExtractor, LoggingDataExtractor>();
        serviceCollection.AddSingleton<ITelemetryInitializer, SSAppInsightsTelemetry>();
        
        serviceCollection.AddScoped<IQueryHandlerFactory, QueryHandlerFactory>();
        serviceCollection.AddScoped<IQueryHandler, QueryHandler>();

        serviceCollection.AddScoped<ICommandHandlerFactory, CommandHandlerFactory>();
        serviceCollection.AddScoped<ICommandHandler, CommandHandler>();
        
        
        // register command handlers
        serviceCollection.Scan(scan => scan.FromAssemblyOf<ICommand>()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>))
                .Where(_ => !_.IsGenericType))
            .AsImplementedInterfaces()
            .WithTransientLifetime());
        serviceCollection.Decorate(typeof(ICommandHandler<>), typeof(CommandHandlerLoggingDecorator<>));

        // register query handlers
        serviceCollection.Scan(scan => scan.FromAssemblyOf<IQuery>()
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>))
                .Where(_ => !_.IsGenericType))
            .AsImplementedInterfaces()
            .WithTransientLifetime());
        serviceCollection.Decorate(typeof(IQueryHandler<,>), typeof(QueryHandlerLoggingDecorator<,>));
        
        return serviceCollection;
    }
}