namespace Api.Middleware;

public class ExceptionMapperMiddleware
{
    private readonly RequestDelegate _next;


    public ExceptionMapperMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (AuthorNotFoundException ex)
        {
            await HandleExceptionAsync(httpContext, HttpStatusCode.NotFound, ex);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, HttpStatusCode.InternalServerError, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int) statusCode;
        return context.Response.WriteAsJsonAsync(exception.Message);
    }
}

public static class ExceptionHandlerMiddlewareExtensions
{
    /// <summary>
    ///     Add the <see cref="ExceptionMapperMiddleware" />
    /// </summary>
    /// <param name="builder">The <see cref="IApplicationBuilder" /> instance</param>
    /// <returns>The <see cref="IApplicationBuilder" /> instance</returns>
    public static IApplicationBuilder UseExceptionMapper(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionMapperMiddleware>();
    }
}