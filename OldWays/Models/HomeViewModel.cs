namespace OldWays.Models
{
    public class HomeViewModel
    {
        //example : send 3 days of forecast to the view
        public List<ForecastDay> ForecastDays { get; set; } = new();
        public List<Slideshow> Slideshows { get; set; } = new();
    }
}
