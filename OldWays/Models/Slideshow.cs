namespace OldWays.Models
{
    public class Slideshow
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }

        public List<SlideshowImage> Images { get; set; } = new();
    }

}
