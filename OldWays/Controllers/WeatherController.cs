using Microsoft.AspNetCore.Mvc;
using OldWays.Services;

namespace OldWays.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {

        private readonly WeatherService _weatherService;

        public WeatherController(WeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet("forecast")]
        public async Task<IActionResult> GetForecast()
        {
            var data = await _weatherService.GetForecastAsync();
            return Ok(data);
        }
    }
}
