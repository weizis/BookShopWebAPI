using System.ComponentModel.DataAnnotations;

namespace API1.DTOs.Book
{
    public class CreateBookDto
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public int AuthorID { get; set; }

        [StringLength(20)]
        public string? ISBN { get; set; }

        public decimal? Price { get; set; }
        public int? Pages { get; set; }
        public int? StockQuantity { get; set; }
        public bool? IsAvailable { get; set; }
    }
}