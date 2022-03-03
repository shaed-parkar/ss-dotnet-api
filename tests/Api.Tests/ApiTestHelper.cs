namespace Api.Tests;

public static class ApiTestHelper
{
    public static StringContent CreateJsonPayloadForRequest(object payload)
    {
        return new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
    }
}