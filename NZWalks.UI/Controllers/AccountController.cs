using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models;

namespace NZWalks.UI.Controllers;

[Route("account")]
public class AccountController(IHttpClientFactory httpClientFactory, ILogger<AccountController> logger)
    : ApiControllerBase(httpClientFactory)
{
    [HttpGet("login")]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View(new LoginViewModel());
    }

    [HttpPost("login")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            using var response = await ApiClient.PostAsJsonAsync("api/auth/login", model);
            if (!response.IsSuccessStatusCode)
            {
                await SetApiErrorAsync(response, "Sign in failed.");
                return View(model);
            }

            var result = await response.Content.ReadFromJsonAsync<LoginResponseDTO>();
            if (string.IsNullOrWhiteSpace(result?.JwtToken))
            {
                ModelState.AddModelError(string.Empty, "The API did not return an access token.");
                return View(model);
            }

            HttpContext.Session.SetString("AccessToken", result.JwtToken);
            HttpContext.Session.SetString("UserName", model.UserName);
            TempData["Success"] = "You are signed in.";
            return Url.IsLocalUrl(returnUrl)
                ? Redirect(returnUrl!)
                : RedirectToAction("Index", "Home");
        }
        catch (HttpRequestException exception)
        {
            logger.LogError(exception, "Could not connect to the NZWalks API while signing in.");
            ModelState.AddModelError(string.Empty, "Could not connect to the API. Start NZWalks.Api and try again.");
            return View(model);
        }
    }

    [HttpGet("register")]
    public IActionResult Register() => View(new RegisterViewModel());

    [HttpPost("register")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            using var response = await ApiClient.PostAsJsonAsync("api/auth/register", new
            {
                model.UserName,
                model.Password,
                Roles = new[] { "Reader" }
            });
            if (!response.IsSuccessStatusCode)
            {
                await SetApiErrorAsync(response, "Account could not be created.");
                return View(model);
            }

            TempData["Success"] = "Your reader account is ready. Sign in to continue.";
            return RedirectToAction(nameof(Login));
        }
        catch (HttpRequestException exception)
        {
            logger.LogError(exception, "Could not connect to the NZWalks API while registering.");
            ModelState.AddModelError(string.Empty, "Could not connect to the API. Start NZWalks.Api and try again.");
            return View(model);
        }
    }

    [HttpPost("logout")]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        TempData["Success"] = "You are signed out.";
        return RedirectToAction("Index", "Home");
    }
}
