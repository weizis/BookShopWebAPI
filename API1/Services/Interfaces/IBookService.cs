
using API1.DTOs.Book;

namespace API1.Services.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<BookDto>> GetAllBooksAsync();
        Task<BookDto?> GetBookByIdAsync(int id);
        Task<IEnumerable<BookDto>> SearchBooksAsync(string title);
        Task<IEnumerable<BookDto>> GetBooksByAuthorAsync(string author);
        Task<BookDto> CreateBookAsync(CreateBookDto dto);
        Task<BookDto> UpdateBookAsync(int id, CreateBookDto dto);
        Task DeleteBookAsync(int id);

    }
}
