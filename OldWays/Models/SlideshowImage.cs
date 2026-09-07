namespace OldWays.Models
{
    public class SlideshowImage
    {
        public int Id { get; set; }
        public int SlideshowId { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string AltText { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }

        public Slideshow Slideshow { get; set; }
    }

}
