namespace OldWays.Models
{
    public class WeatherSettings
    {
        public string ApiKey { get; set; } = string.Empty;
        public string City { get; set; } = "Toronto";
        public int Days { get; set; } = 3;
    }
}
//TODO: add to settings panel.
