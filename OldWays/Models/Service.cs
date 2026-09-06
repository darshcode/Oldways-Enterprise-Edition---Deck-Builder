using System.ComponentModel.DataAnnotations;

namespace OldWays.Models
{
    public class Service
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        public string? Time { get; set; }

        [Required]
        public string Category { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }
}
