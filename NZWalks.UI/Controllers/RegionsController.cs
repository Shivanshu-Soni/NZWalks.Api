using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NZWalks.UI.Models;
using NZWalks.UI.Models.DTO;

namespace NZWalks.UI.Controllers
{
    [Route("regions")]
    public class RegionsController : Controller
    {
        private readonly ILogger<RegionsController> _logger;
        public IHttpClientFactory HttpClientFactory;

        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            this.HttpClientFactory = httpClientFactory;
        }

        [HttpGet] // Matches GET requests to /regions
        public async Task<IActionResult> Index()
        {
            List<RegionDTO> response = new List<RegionDTO>();
            try
            {
                var client = HttpClientFactory.CreateClient();
                var httpResponseMessage = await client.GetAsync("http://localhost:5005/api/regions");
                httpResponseMessage.EnsureSuccessStatusCode();
                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDTO>>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching regions");
            }

            return View(response);
        }

        [HttpGet("add")] // Matches POST requests to /regions/add
        public async Task<IActionResult> Add()
        {
            return View();
        }

          [HttpPost("add")] // Matches POST requests to /regions/add
        public async Task<IActionResult> Add(AddRegionViewModel addRegionViewModel)
        {
            var client = HttpClientFactory.CreateClient();
            var httpRequestMessage = new HttpRequestMessage(){
                Method= HttpMethod.Post,
                RequestUri = new Uri("http://localhost:5005/api/regions"),
                Content = new StringContent(JsonSerializer.Serialize(addRegionViewModel), Encoding.UTF8, "application/json"),

            };

            var httpResponseMessage = await  client.SendAsync(httpRequestMessage);
            httpResponseMessage.EnsureSuccessStatusCode();
           var response =  await httpResponseMessage.Content.ReadFromJsonAsync<RegionDTO>();
           if(response != null){
            return RedirectToAction("Index", "Region");
           }

            return View();
        }
        
    }
}
