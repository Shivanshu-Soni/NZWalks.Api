using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace NZWalks.UI.Controllers
{
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        private readonly ILogger<RegionsController> _logger;
        public IHttpClientFactory HttpClientFactory;

        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            this.HttpClientFactory = httpClientFactory;

        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var client = HttpClientFactory.CreateClient();
                var httpResponseMessage = await client.GetAsync("https://localhost:7053/api/regions");
                httpResponseMessage.EnsureSuccessStatusCode();
                var stringResponseBody = await httpResponseMessage.Content.ReadAsStringAsync();
                ViewBag.response = stringResponseBody;
            }
            catch (Exception ex)
            {

            }

            return View();

        }
    }
}