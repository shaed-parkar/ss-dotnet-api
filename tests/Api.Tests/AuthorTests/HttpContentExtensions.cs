using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Api.Tests.AuthorTests;

public static class HttpContentExtensions
{
    public static async Task<T> DeserialiseTo<T>(this HttpContent httpContent)
    {
        var json = await httpContent.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(json)!;
    }
}