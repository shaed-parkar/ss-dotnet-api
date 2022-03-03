namespace SS.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Register types to the IoC
    /// </summary>
    /// <param name="serviceCollection">The <see cref="IServiceCollection" /></param>
    public static void AddCustomTypes(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ILoggingDataExtractor, LoggingDataExtractor>();

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

        serviceCollection.AddTransient<IRequestModelValidatorService, RequestModelValidatorService>();
        serviceCollection.AddTransient<IValidatorFactory, RequestModelValidatorFactory>();
    }

    /// <summary>
    ///     Add the swagger page
    /// </summary>
    /// <param name="serviceCollection">The <see cref="IServiceCollection" /></param>
    public static void AddSwagger(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSwaggerGen(options =>
        {
            var contractsXmlFileName = $"{typeof(AuthorDto).Assembly.GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, contractsXmlFileName));

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
    }
}