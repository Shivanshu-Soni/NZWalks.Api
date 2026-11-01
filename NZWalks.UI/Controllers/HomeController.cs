using System.Diagnostics;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models;
using NZWalks.UI.Models.DTO;

namespace NZWalks.UI.Controllers;

public class HomeController(
    ILogger<HomeController> logger,
    IHttpClientFactory httpClientFactory) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    public async Task<IActionResult> Index()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("NZWalksApi");
            using var regionsResponse = await client.GetAsync("api/regions");
            using var walksResponse = await client.GetAsync("api/walks");
            ViewBag.RegionCount = regionsResponse.IsSuccessStatusCode
                ? (await regionsResponse.Content.ReadFromJsonAsync<List<RegionDTO>>())?.Count
                : null;
            ViewBag.WalkCount = walksResponse.IsSuccessStatusCode
                ? (await walksResponse.Content.ReadFromJsonAsync<List<WalkDTO>>())?.Count
                : null;
        }
        catch (HttpRequestException exception)
        {
            _logger.LogWarning(exception, "Could not connect to the NZWalks API for homepage statistics.");
            ViewBag.ApiUnavailable = true;
        }

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
