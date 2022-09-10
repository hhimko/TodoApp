using System.Net.Http.Json;
using System.Text.Json;
using System.Net;

using TodoAppLib.Middleware.StatusCodeExceptionMiddleware;

namespace TodoAppLib.ExtensionMethods;

public static class HttpClientExtensionMethods
{
    public static async Task<T?> GetAsyncFromAPI<T>(this HttpClient client, string url)
    {
        var response = await client.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            throw new StatusCodeException(HttpStatusCode.InternalServerError, "Connecion to API failed");

        try
        {
            return await response.Content.ReadFromJsonAsync<T>();
        }
        catch (JsonException)
        {
            throw new StatusCodeException(HttpStatusCode.InternalServerError, "Connecion to API failed");
        }
    }
}
