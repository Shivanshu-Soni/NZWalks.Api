using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;

namespace NZWalks.UI.Controllers;

public abstract class ApiControllerBase(IHttpClientFactory httpClientFactory) : Controller
{
    protected HttpClient ApiClient
    {
        get
        {
            var client = httpClientFactory.CreateClient("NZWalksApi");
            var token = HttpContext.Session.GetString("AccessToken");
            if (!string.IsNullOrWhiteSpace(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return client;
        }
    }

    protected async Task SetApiErrorAsync(HttpResponseMessage response, string fallback)
    {
        var details = await response.Content.ReadAsStringAsync();
        TempData["Error"] = string.IsNullOrWhiteSpace(details)
            ? fallback
            : $"{fallback} ({(int)response.StatusCode}): {details}";
    }
}
