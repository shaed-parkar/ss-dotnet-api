using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;

namespace SS.Common;

public class SSAppInsightsTelemetry : ITelemetryInitializer
{
    readonly IHttpContextAccessor _httpContextAccessor;

    public SSAppInsightsTelemetry(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void Initialize(ITelemetry telemetry)
    {
        telemetry.Context.Cloud.RoleName = "ss-basic-api";
            
        if (telemetry is not RequestTelemetry requestTelemetry)
        {
            return;
        }

        if (!IsReadableBadRequest(requestTelemetry))
        {
            return;
        }

        // Check response body
        var responseBody = (string?) _httpContextAccessor.HttpContext?.Items["responseBody"];
        if (responseBody != null)
        {
            requestTelemetry.Properties.Add("responseBody", responseBody);
        }
    }

    private bool IsReadableBadRequest(RequestTelemetry telemetry)
    {
        return _httpContextAccessor.HttpContext!.Request.Body.CanRead
               && telemetry.ResponseCode == "400";
    }
}