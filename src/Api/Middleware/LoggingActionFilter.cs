#pragma warning disable CS8604

namespace SS.Middleware;

public class LoggingActionFilter : IAsyncActionFilter
{
    private readonly ILogger<LoggingActionFilter> _logger;

    private readonly ILoggingDataExtractor _loggingDataExtractor;

    public LoggingActionFilter(ILogger<LoggingActionFilter> logger, ILoggingDataExtractor loggingDataExtractor)
    {
        _logger = logger;
        _loggingDataExtractor = loggingDataExtractor;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var properties = context.ActionDescriptor.Parameters
            .Select(p => context.ActionArguments.SingleOrDefault(x => x.Key == p.Name))
            .SelectMany(pv => _loggingDataExtractor.ConvertToDictionary(pv.Value, pv.Key))
            .ToDictionary(x => x.Key, x => x.Value);

        if (context.ActionDescriptor is ControllerActionDescriptor actionDescriptor)
        {
            properties.Add(nameof(actionDescriptor.ControllerName), actionDescriptor.ControllerName);
            properties.Add(nameof(actionDescriptor.ActionName), actionDescriptor.ActionName);
            properties.Add(nameof(actionDescriptor.DisplayName), actionDescriptor.DisplayName);
        }

        using (_logger.BeginScope(properties))
        {
            await next();
        }
    }
}