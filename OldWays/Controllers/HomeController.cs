using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OldWays.Models;
using System.Diagnostics;
using System.Text.Json;

namespace OldWays.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            var apiKey = _configuration["WeatherSettings:ApiKey"];
            var city = _configuration["WeatherSettings:City"];
            var days = _configuration["WeatherSettings:Days"];

            var url = $"https://api.weatherapi.com/v1/forecast.json?key={apiKey}&q={city}&days={days}&aqi=no&alerts=no";

            using var client = new HttpClient();
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Weather API failed with status: " + response.StatusCode);
                return View(new WeatherForecastViewModel());
            }

            var json = await response.Content.ReadAsStringAsync();
            var apiData = JsonSerializer.Deserialize<WeatherApiResponse>(json, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return View(new WeatherForecastViewModel
            {
                ForecastDays = apiData?.Forecast?.Forecastday ?? new List<ForecastDay>()
            });
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
}
