using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Api.Middleware.Validation;

public class RequestModelValidatorFilter : IAsyncActionFilter
{
    private readonly ILogger<RequestModelValidatorFilter> _logger;
    private readonly IRequestModelValidatorService _requestModelValidatorService;

    public RequestModelValidatorFilter(IRequestModelValidatorService requestModelValidatorService,
        ILogger<RequestModelValidatorFilter> logger)
    {
        _requestModelValidatorService = requestModelValidatorService;
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        _logger.LogDebug("Processing request");
        foreach (var property in context.ActionDescriptor.Parameters)
        {
            var valuePair = context.ActionArguments.SingleOrDefault(x => x.Key == property.Name);
            if (property.BindingInfo?.BindingSource == BindingSource.Body)
            {
                var validationFailures =
                    _requestModelValidatorService.Validate(property.ParameterType, valuePair.Value);
                context.ModelState.AddFluentValidationErrors(validationFailures);
            }

            if (valuePair.Value.Equals(GetDefaultValue(property.ParameterType)))
                context.ModelState.AddModelError(valuePair.Key, $"Please provide a valid {valuePair.Key}");
        }

        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage)).ToList();
            var errorsString = string.Join("; ", errors);
            _logger.LogWarning("Request Validation Failed: {Errors}", errorsString);
            context.Result = new BadRequestObjectResult(context.ModelState.Values);
        }
        else
        {
            await next();
        }
    }

    private static object GetDefaultValue(Type type)
    {
        return type.IsValueType ? Activator.CreateInstance(type) : null;
    }
}