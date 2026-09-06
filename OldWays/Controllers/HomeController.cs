using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OldWays.Data;
using OldWays.Models;
using OldWays.Services;
using System.Diagnostics;
using System.Text.Json;

namespace OldWays.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly WeatherSettings _weatherSettings;
        private readonly WeatherService _weatherService;
        private readonly ApplicationDbContext _db;


        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, IOptions<WeatherSettings> weatherOptions, WeatherService weatherService, ApplicationDbContext db)
        {
            _logger = logger;
            _configuration = configuration;
            _weatherSettings = weatherOptions.Value;
            _weatherService = weatherService;
            _db = db;
        }

        public async Task<IActionResult> Index()
        {

            var forecast = await _weatherService.GetForecastAsync();

            /*
             var slideshows = await _db.Slideshows
            .Where(s => s.IsActive)
            .OrderBy(s => s.DisplayOrder)
            .Include(s => s.Images.Where(i => i.IsActive))
            .ToListAsync();
            */

            var vm = new HomeViewModel
            {
                ForecastDays = forecast,
                //Slideshows = slideshows
            };

            return View(vm);

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
