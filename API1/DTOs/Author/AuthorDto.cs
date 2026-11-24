namespace API1.DTOs.Author
{
    public class AuthorDto
    {
        public int AuthorId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Country { get; set; }
        public int BooksCount { get; set; } 
    }
}