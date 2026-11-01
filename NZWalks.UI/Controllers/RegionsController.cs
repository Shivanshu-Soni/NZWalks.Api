using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models;
using NZWalks.UI.Models.DTO;

namespace NZWalks.UI.Controllers;

[Route("regions")]
public class RegionsController(IHttpClientFactory httpClientFactory, ILogger<RegionsController> logger)
    : ApiControllerBase(httpClientFactory)
{
    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        try
        {
            using var response = await ApiClient.GetAsync("api/regions");
            if (!response.IsSuccessStatusCode)
            {
                await SetApiErrorAsync(response, "Regions could not be loaded.");
                return View(Array.Empty<RegionDTO>());
            }

            var regions = await response.Content.ReadFromJsonAsync<List<RegionDTO>>();
            return View(regions ?? []);
        }
        catch (HttpRequestException exception)
        {
            logger.LogError(exception, "Could not connect to the NZWalks API while loading regions.");
            TempData["Error"] = "Could not connect to the API. Start NZWalks.Api and try again.";
            return View(Array.Empty<RegionDTO>());
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Details(Guid id)
    {
        try
        {
            using var response = await ApiClient.GetAsync($"api/regions/{id}");
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound();
            }

            if (!response.IsSuccessStatusCode)
            {
                await SetApiErrorAsync(response, "Region details could not be loaded.");
                return RedirectToAction(nameof(Index));
            }

            var region = await response.Content.ReadFromJsonAsync<RegionDTO>();
            return region is null ? NotFound() : View(region);
        }
        catch (HttpRequestException exception)
        {
            logger.LogError(exception, "Could not connect to the NZWalks API while loading region {RegionId}.", id);
            TempData["Error"] = "Could not connect to the API. Start NZWalks.Api and try again.";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpGet("add")]
    public IActionResult Add() => View(new AddRegionViewModel());

    [HttpPost("add")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(AddRegionViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            using var response = await ApiClient.PostAsJsonAsync("api/regions", model);
            if (!response.IsSuccessStatusCode)
            {
                await SetApiErrorAsync(response, "Region could not be created. A Writer account is required.");
                return View(model);
            }

            TempData["Success"] = "Region added successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch (HttpRequestException exception)
        {
            logger.LogError(exception, "Could not connect to the NZWalks API while creating a region.");
            ModelState.AddModelError(string.Empty, "Could not connect to the API. Start NZWalks.Api and try again.");
            return View(model);
        }
    }

    [HttpGet("{id:guid}/edit")]
    public async Task<IActionResult> Edit(Guid id)
    {
        try
        {
            using var response = await ApiClient.GetAsync($"api/regions/{id}");
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound();
            }

            if (!response.IsSuccessStatusCode)
            {
                await SetApiErrorAsync(response, "Region could not be loaded.");
                return RedirectToAction(nameof(Index));
            }

            var region = await response.Content.ReadFromJsonAsync<RegionDTO>();
            return region is null
                ? NotFound()
                : View(new AddRegionViewModel
                {
                    Code = region.Code,
                    Name = region.Name,
                    RegionImageUrl = region.RegionImageUrl
                });
        }
        catch (HttpRequestException exception)
        {
            logger.LogError(exception, "Could not connect to the NZWalks API while loading region {RegionId}.", id);
            TempData["Error"] = "Could not connect to the API. Start NZWalks.Api and try again.";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost("{id:guid}/edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, AddRegionViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            using var response = await ApiClient.PutAsJsonAsync($"api/regions/{id}", model);
            if (!response.IsSuccessStatusCode)
            {
                await SetApiErrorAsync(response, "Region could not be updated. A Writer account is required.");
                return View(model);
            }

            TempData["Success"] = "Region updated successfully.";
            return RedirectToAction(nameof(Details), new { id });
        }
        catch (HttpRequestException exception)
        {
            logger.LogError(exception, "Could not connect to the NZWalks API while updating region {RegionId}.", id);
            ModelState.AddModelError(string.Empty, "Could not connect to the API. Start NZWalks.Api and try again.");
            return View(model);
        }
    }

    [HttpPost("{id:guid}/delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            using var response = await ApiClient.DeleteAsync($"api/regions/{id}");
            if (!response.IsSuccessStatusCode)
            {
                await SetApiErrorAsync(response, "Region could not be deleted. A Writer account is required.");
            }
            else
            {
                TempData["Success"] = "Region deleted successfully.";
            }
        }
        catch (HttpRequestException exception)
        {
            logger.LogError(exception, "Could not connect to the NZWalks API while deleting region {RegionId}.", id);
            TempData["Error"] = "Could not connect to the API. Start NZWalks.Api and try again.";
        }

        return RedirectToAction(nameof(Index));
    }
}
