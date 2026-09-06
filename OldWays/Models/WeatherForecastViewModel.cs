namespace OldWays.Models
{
    //list of forecast days to send to the view
    public class WeatherForecastViewModel
    {
        public List<ForecastDay> ForecastDays { get; set; } = new();
    }

    //weather api response
    public class WeatherApiResponse
    {
        public Forecast Forecast { get; set; }
    }

    //list of forecast days
    public class Forecast
    {
        public List<ForecastDay> Forecastday { get; set; }
    }

    //one forecast day
    public class ForecastDay
    {
        public string Date { get; set; }
        public Day Day { get; set; }
    }

    //response formatting
    public class Condition
    {
        public string Text { get; set; }
        public string Icon { get; set; }
    }
}

