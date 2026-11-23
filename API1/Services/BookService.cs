
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
                AuthorName = b.Author.FirstName + " " + b.Author.LastName,
                ISBN = b.Isbn,
                Price = b.Price,
                Pages = b.Pages,
                StockQuantity = b.StockQuantity,
                IsAvailable = b.IsAvailable
            });
        }

        public async Task<BookDto?> GetBookByIdAsync(int id)
        {
            var book = await _context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(b => b.BookId == id);

            if (book == null)
                return null;

            return new BookDto
            {
                BookID = book.BookId,
                Title = book.Title,
                AuthorID = book.AuthorId,
                AuthorName = book.Author.FirstName + " " + book.Author.LastName,
                ISBN = book.Isbn,
                Price = book.Price,
                Pages = book.Pages,
                StockQuantity = book.StockQuantity,
                IsAvailable = book.IsAvailable
            };
        }


        public async Task<IEnumerable<BookDto>> SearchBooksAsync(string title)
        {
            var books = await _context.Books
                .Include(b => b.Author)
                .Where(b => b.Title.Contains(title))
                .ToListAsync();

            return books.Select(b => new BookDto
            {
                BookID = b.BookId,
                Title = b.Title,
                AuthorID = b.AuthorId,
                AuthorName = b.Author.FirstName + " " + b.Author.LastName,
                ISBN = b.Isbn,
                Price = b.Price,
                Pages = b.Pages,
                StockQuantity = b.StockQuantity,
                IsAvailable = b.IsAvailable
            });
        }

        public async Task<IEnumerable<BookDto>> GetBooksByAuthorAsync(string author)
        {
            var books = await _context.Books
                .Include(b => b.Author)
                .Where(b => b.Author.FirstName.Contains(author) ||
                            b.Author.LastName.Contains(author) ||
                            (b.Author.FirstName + " " + b.Author.LastName).Contains(author))
                .ToListAsync();

            return books.Select(b => new BookDto
            {
                BookID = b.BookId,
                Title = b.Title,
                AuthorID = b.AuthorId,
                AuthorName = b.Author.FirstName + " " + b.Author.LastName,
                ISBN = b.Isbn,
                Price = b.Price,
                Pages = b.Pages,
                StockQuantity = b.StockQuantity,
                IsAvailable = b.IsAvailable
            });
        }


        public async Task<BookDto> CreateBookAsync(CreateBookDto dto)
        {
            var authorExists = await _context.Authors
                .AnyAsync(a => a.AuthorId == dto.AuthorID);

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

            return await GetBookByIdAsync(book.BookId)
                ?? throw new Exception("Ошибка при создании книги");
        }

        public async Task<BookDto> UpdateBookAsync(int id, CreateBookDto dto)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
                throw new NotFoundException($"Книга с ID {id} не найдена");

            var authorExists = await _context.Authors
                .AnyAsync(a => a.AuthorId == dto.AuthorID);

            if (!authorExists)
                throw new BadRequestException($"Автор с ID {dto.AuthorID} не найден");

            book.Title = dto.Title;
            book.AuthorId = dto.AuthorID;
            book.Isbn = dto.ISBN;
            book.Price = dto.Price ?? 0;
            book.Pages = dto.Pages;
            book.StockQuantity = dto.StockQuantity;
            book.IsAvailable = dto.IsAvailable;

            await _context.SaveChangesAsync();

            return await GetBookByIdAsync(id)
                ?? throw new Exception("Ошибка при обновлении книги");
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
