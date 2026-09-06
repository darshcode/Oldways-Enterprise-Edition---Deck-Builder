using Microsoft.Extensions.Options;
using OldWays.Models;
using System.Text.Json;

namespace OldWays.Services
{
    public class WeatherService
    {
        private readonly WeatherSettings _settings;
        private readonly ILogger<WeatherService> _logger;
        private readonly HttpClient _httpClient;

        public WeatherService(IOptions<WeatherSettings> settings, ILogger<WeatherService> logger, HttpClient httpClient)
        {
            _settings = settings.Value;
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<List<ForecastDay>> GetForecastAsync()
        {
            var apiKey = _settings.ApiKey;
            var city = _settings.City;
            var days = _settings.Days;

            var url = $"https://api.weatherapi.com/v1/forecast.json?key={apiKey}&q={city}&days={days}&aqi=no&alerts=no";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Weather API failed with status: " + response.StatusCode);
                return new List<ForecastDay>();
            }

            var json = await response.Content.ReadAsStringAsync();
            var apiData = JsonSerializer.Deserialize<WeatherApiResponse>(json,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            return apiData?.Forecast?.Forecastday ?? new List<ForecastDay>();
        }
    }
}
