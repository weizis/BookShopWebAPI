using API1.DTOs.Book;
using API1.Exceptions;
using API1.Models;
using API1.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API1.Services
{
    public class BookService : IBookService
    {
        private readonly BookShopContext _context;

        public BookService(BookShopContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BookDto>> GetAllBooksAsync()
        {
            var books = await _context.Books
                .Include(b => b.Author)
                .ToListAsync();

            return books.Select(b => new BookDto
            {
                BookID = b.BookId,
                Title = b.Title,
                AuthorID = b.AuthorId,
                AuthorName = b.Author != null ? $"{b.Author.FirstName} {b.Author.LastName}" : "Неизвестен",
                ISBN = b.Isbn,
                Price = b.Price,
                Pages = b.Pages,
                StockQuantity = b.StockQuantity,
                IsAvailable = b.IsAvailable
            });
        }

        public async Task<BookDto?> GetBookByIdAsync(int id)
        {
            var b = await _context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(b => b.BookId == id); 

            if (b == null) return null;

            return new BookDto
            {
                BookID = b.BookId,
                Title = b.Title,
                AuthorID = b.AuthorId,
                AuthorName = b.Author != null ? $"{b.Author.FirstName} {b.Author.LastName}" : "Неизвестен",
                ISBN = b.Isbn,
                Price = b.Price,
                Pages = b.Pages,
                StockQuantity = b.StockQuantity,
                IsAvailable = b.IsAvailable
            };
        }

        public async Task<BookDto> CreateBookAsync(CreateBookDto dto)
        {
            var authorExists = await _context.Authors.AnyAsync(a => a.AuthorId == dto.AuthorID); // Исправлено на AuthorId
            if (!authorExists)
                throw new BadRequestException($"Автор с ID {dto.AuthorID} не найден");

            var book = new Book
            {
                Title = dto.Title,
                AuthorId = dto.AuthorID, 
                Isbn = dto.ISBN,         
                Price = dto.Price ?? 0, 
                Pages = dto.Pages,
                StockQuantity = dto.StockQuantity,
                IsAvailable = dto.IsAvailable
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return await GetBookByIdAsync(book.BookId) ?? throw new Exception("Ошибка при создании"); 
        }

        public async Task<BookDto> UpdateBookAsync(int id, CreateBookDto dto)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
                throw new NotFoundException($"Книга с ID {id} не найдена");

            book.Title = dto.Title;
            book.AuthorId = dto.AuthorID; 
            book.Isbn = dto.ISBN;         
            book.Price = dto.Price ?? 0;  
            book.Pages = dto.Pages;
            book.StockQuantity = dto.StockQuantity;
            book.IsAvailable = dto.IsAvailable;

            await _context.SaveChangesAsync();

            return await GetBookByIdAsync(id)!;
        }

        public async Task DeleteBookAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
                throw new NotFoundException($"Книга с ID {id} не найдена");

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }
    }
}