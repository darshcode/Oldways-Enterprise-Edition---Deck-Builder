namespace OldWays.Models
{    
        public class WeatherForecastViewModel
        {
            public List<ForecastDay> ForecastDays { get; set; } = new();
        }

        public class WeatherApiResponse
        {
            public Forecast Forecast { get; set; }
        }

        public class Forecast
        {
            public List<ForecastDay> Forecastday { get; set; }
        }

        public class ForecastDay
        {
            public string Date { get; set; }
            public Day Day { get; set; }
        }

        public class Condition
        {
            public string Text { get; set; }
            public string Icon { get; set; }
        }
    }

