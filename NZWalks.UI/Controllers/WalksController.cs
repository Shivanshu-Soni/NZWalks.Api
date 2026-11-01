using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models;
using NZWalks.UI.Models.DTO;

namespace NZWalks.UI.Controllers;

[Route("walks")]
public class WalksController(IHttpClientFactory httpClientFactory, ILogger<WalksController> logger)
    : ApiControllerBase(httpClientFactory)
{
    [HttpGet("")]
    public async Task<IActionResult> Index(string? search, string? sortBy, bool descending = false)
    {
        ViewBag.Search = search;
        ViewBag.SortBy = sortBy;
        ViewBag.Descending = descending;

        var query = new List<string>();
        if (!string.IsNullOrWhiteSpace(search))
        {
            query.Add($"filterOn=Name&filterQuery={Uri.EscapeDataString(search)}");
        }
        if (!string.IsNullOrWhiteSpace(sortBy))
        {
            query.Add($"sortBy={Uri.EscapeDataString(sortBy)}&isAscending={!descending}");
        }

        var path = "api/walks" + (query.Count == 0 ? string.Empty : $"?{string.Join("&", query)}");
        try
        {
            using var response = await ApiClient.GetAsync(path);
            if (!response.IsSuccessStatusCode)
            {
                await SetApiErrorAsync(response, "Walks could not be loaded.");
                return View(Array.Empty<WalkDTO>());
            }

            var walks = await response.Content.ReadFromJsonAsync<List<WalkDTO>>();
            return View(walks ?? []);
        }
        catch (HttpRequestException exception)
        {
            logger.LogError(exception, "Could not connect to the NZWalks API while loading walks.");
            TempData["Error"] = "Could not connect to the API. Start NZWalks.Api and try again.";
            return View(Array.Empty<WalkDTO>());
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Details(Guid id)
    {
        try
        {
            using var response = await ApiClient.GetAsync($"api/walks/{id}");
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound();
            }
            if (!response.IsSuccessStatusCode)
            {
                await SetApiErrorAsync(response, "Walk details could not be loaded.");
                return RedirectToAction(nameof(Index));
            }

            var walk = await response.Content.ReadFromJsonAsync<WalkDTO>();
            return walk is null ? NotFound() : View(walk);
        }
        catch (HttpRequestException exception)
        {
            logger.LogError(exception, "Could not connect to the NZWalks API while loading walk {WalkId}.", id);
            TempData["Error"] = "Could not connect to the API. Start NZWalks.Api and try again.";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpGet("add")]
    public async Task<IActionResult> Add()
    {
        var model = await CreateWalkViewModelAsync();
        return View(model);
    }

    [HttpPost("add")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(WalkViewModel model)
    {
        ValidateWalkSelections(model);
        if (!ModelState.IsValid)
        {
            await PopulateRegionsAsync(model);
            return View(model);
        }

        try
        {
            using var response = await ApiClient.PostAsJsonAsync("api/walks", model);
            if (!response.IsSuccessStatusCode)
            {
                await SetApiErrorAsync(response, "Walk could not be created.");
                await PopulateRegionsAsync(model);
                return View(model);
            }

            var walk = await response.Content.ReadFromJsonAsync<WalkDTO>();
            if (walk is null)
            {
                TempData["Error"] = "The API created the walk but returned no walk details.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "Walk added successfully.";
            return RedirectToAction(nameof(Details), new { id = walk.Id });
        }
        catch (HttpRequestException exception)
        {
            logger.LogError(exception, "Could not connect to the NZWalks API while creating a walk.");
            ModelState.AddModelError(string.Empty, "Could not connect to the API. Start NZWalks.Api and try again.");
            await PopulateRegionsAsync(model);
            return View(model);
        }
    }

    [HttpGet("{id:guid}/edit")]
    public async Task<IActionResult> Edit(Guid id)
    {
        try
        {
            using var response = await ApiClient.GetAsync($"api/walks/{id}");
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound();
            }
            if (!response.IsSuccessStatusCode)
            {
                await SetApiErrorAsync(response, "Walk could not be loaded.");
                return RedirectToAction(nameof(Index));
            }

            var walk = await response.Content.ReadFromJsonAsync<WalkDTO>();
            if (walk is null)
            {
                return NotFound();
            }

            var model = await CreateWalkViewModelAsync();
            model.Id = walk.Id;
            model.Name = walk.Name;
            model.Description = walk.Description;
            model.LenghtInkm = walk.LenghtInkm;
            model.WalkImageUrl = walk.WalkImageUrl;
            model.RegionId = walk.Region?.Id ?? Guid.Empty;
            model.DifficultyId = walk.Difficulty?.Id ?? Guid.Empty;
            return View(model);
        }
        catch (HttpRequestException exception)
        {
            logger.LogError(exception, "Could not connect to the NZWalks API while loading walk {WalkId}.", id);
            TempData["Error"] = "Could not connect to the API. Start NZWalks.Api and try again.";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost("{id:guid}/edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, WalkViewModel model)
    {
        ValidateWalkSelections(model);
        if (!ModelState.IsValid)
        {
            await PopulateRegionsAsync(model);
            return View(model);
        }

        try
        {
            using var response = await ApiClient.PutAsJsonAsync($"api/walks/{id}", model);
            if (!response.IsSuccessStatusCode)
            {
                await SetApiErrorAsync(response, "Walk could not be updated.");
                await PopulateRegionsAsync(model);
                return View(model);
            }

            TempData["Success"] = "Walk updated successfully.";
            return RedirectToAction(nameof(Details), new { id });
        }
        catch (HttpRequestException exception)
        {
            logger.LogError(exception, "Could not connect to the NZWalks API while updating walk {WalkId}.", id);
            ModelState.AddModelError(string.Empty, "Could not connect to the API. Start NZWalks.Api and try again.");
            await PopulateRegionsAsync(model);
            return View(model);
        }
    }

    [HttpPost("{id:guid}/delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            using var response = await ApiClient.DeleteAsync($"api/walks/{id}");
            if (!response.IsSuccessStatusCode)
            {
                await SetApiErrorAsync(response, "Walk could not be deleted.");
            }
            else
            {
                TempData["Success"] = "Walk deleted successfully.";
            }
        }
        catch (HttpRequestException exception)
        {
            logger.LogError(exception, "Could not connect to the NZWalks API while deleting walk {WalkId}.", id);
            TempData["Error"] = "Could not connect to the API. Start NZWalks.Api and try again.";
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task<WalkViewModel> CreateWalkViewModelAsync()
    {
        var model = new WalkViewModel();
        await PopulateRegionsAsync(model);
        return model;
    }

    private async Task<bool> PopulateRegionsAsync(WalkViewModel model)
    {
        try
        {
            using var response = await ApiClient.GetAsync("api/regions");
            if (!response.IsSuccessStatusCode)
            {
                await SetApiErrorAsync(response, "Regions could not be loaded for the walk form.");
                return false;
            }

            model.Regions = await response.Content.ReadFromJsonAsync<List<RegionDTO>>() ?? [];
            return true;
        }
        catch (HttpRequestException exception)
        {
            logger.LogError(exception, "Could not connect to the NZWalks API while loading walk form regions.");
            ModelState.AddModelError(string.Empty, "Could not connect to the API. Start NZWalks.Api and try again.");
            return false;
        }

    }

    private void ValidateWalkSelections(WalkViewModel model)
    {
        if (model.RegionId == Guid.Empty)
        {
            ModelState.AddModelError(nameof(model.RegionId), "Choose a region.");
        }

        if (model.DifficultyId == Guid.Empty)
        {
            ModelState.AddModelError(nameof(model.DifficultyId), "Choose a difficulty.");
        }
    }
}
