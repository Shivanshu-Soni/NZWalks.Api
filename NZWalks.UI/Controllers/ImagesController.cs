using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models;

namespace NZWalks.UI.Controllers;

[Route("images")]
public class ImagesController(IHttpClientFactory httpClientFactory, ILogger<ImagesController> logger)
    : ApiControllerBase(httpClientFactory)
{
    [HttpGet("")]
    public IActionResult Upload() => View(new ImageUploadViewModel());

    [HttpPost("")]
    [ValidateAntiForgeryToken]
    [RequestSizeLimit(11 * 1024 * 1024)]
    public async Task<IActionResult> Upload(ImageUploadViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var extension = Path.GetExtension(model.File!.FileName);
        if (!new[] { ".jpg", ".jpeg", ".png" }.Contains(extension, StringComparer.OrdinalIgnoreCase))
        {
            ModelState.AddModelError(nameof(model.File), "Choose a JPG, JPEG, or PNG image.");
            return View(model);
        }

        if (model.File.Length > 10 * 1024 * 1024)
        {
            ModelState.AddModelError(nameof(model.File), "The image must be smaller than 10 MB.");
            return View(model);
        }

        try
        {
            using var form = new MultipartFormDataContent();
            await using var fileStream = model.File.OpenReadStream();
            using var fileContent = new StreamContent(fileStream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(
                string.IsNullOrWhiteSpace(model.File.ContentType) ? "application/octet-stream" : model.File.ContentType);
            var safeFileName = $"{Path.GetFileNameWithoutExtension(model.File.FileName)}{extension.ToLowerInvariant()}";
            form.Add(fileContent, "File", safeFileName);
            form.Add(new StringContent(model.FileName.Trim()), "FileName");
            form.Add(new StringContent(model.FileDescription ?? string.Empty), "FileDescripstion");

            using var response = await ApiClient.PostAsync("api/images/upload", form);
            if (!response.IsSuccessStatusCode)
            {
                await SetApiErrorAsync(response, "Image could not be uploaded.");
                return View(model);
            }

            var image = await response.Content.ReadFromJsonAsync<ImageResponseDTO>();
            if (image is null)
            {
                ModelState.AddModelError(string.Empty, "The API returned no image details.");
                return View(model);
            }

            TempData["Success"] = $"Image uploaded: {image.FileName}";
            ViewBag.ImagePath = image.FilePath;
            ModelState.Clear();
            return View(new ImageUploadViewModel());
        }
        catch (HttpRequestException exception)
        {
            logger.LogError(exception, "Could not connect to the NZWalks API while uploading an image.");
            ModelState.AddModelError(string.Empty, "Could not connect to the API. Start NZWalks.Api and try again.");
            return View(model);
        }
    }
}
