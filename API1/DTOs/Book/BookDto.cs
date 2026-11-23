namespace API1.DTOs.Book
{
    public class BookDto
    {
        public int BookID { get; set; }
        public string Title { get; set; } = string.Empty;
        public int AuthorID { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public string? ISBN { get; set; }
        public decimal? Price { get; set; }
        public int? Pages { get; set; }
        public int? StockQuantity { get; set; }
        public bool? IsAvailable { get; set; }
    }
}