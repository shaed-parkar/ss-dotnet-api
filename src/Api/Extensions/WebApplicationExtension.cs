namespace Api.Extensions;

public static class WebApplicationExtension
{
    /// <summary>
    /// Add the health check endpoint
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance</param>
    public static void AddHealthCheck(this WebApplication app)
    {
        app.UseRouting().UseEndpoints(endpoints =>
        {
            endpoints.MapHealthChecks("/health", new HealthCheckOptions
            {
                AllowCachingResponses = false,
                ResponseWriter = WriteHealthCheckResponse
            });
        });
    }
    
    private static Task WriteHealthCheckResponse(HttpContext context, HealthReport result)
    {
        context.Response.ContentType = "application/json";

        var json = new JObject(
            new JProperty("status", result.Status.ToString()),
            new JProperty("results", new JObject(result.Entries.Select(pair =>
                new JProperty(pair.Key, new JObject(
                    new JProperty("status", pair.Value.Status.ToString()),
                    new JProperty("description", pair.Value.Description),
                    new JProperty("data", new JObject(pair.Value.Data.Select(
                        p => new JProperty(p.Key, p.Value))))))))));

        return context.Response.WriteAsync(
            json.ToString(Formatting.Indented));
    }
}