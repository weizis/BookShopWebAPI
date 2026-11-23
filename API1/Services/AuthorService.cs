using API1.DTOs.Author;
using API1.Exceptions;
using API1.Models; // Ваши модели из БД
using API1.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API1.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly BookShopContext _context;

        public AuthorService(BookShopContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AuthorDto>> GetAllAuthorsAsync()
        {
            var authors = await _context.Authors
                .Include(a => a.Books) // Подгружаем книги, чтобы посчитать их количество
                .ToListAsync();

            return authors.Select(a => new AuthorDto
            {
                AuthorId = a.AuthorId,
                FirstName = a.FirstName,
                LastName = a.LastName,
                Country = a.Country,
                BooksCount = a.Books.Count // Считаем количество книг
            });
        }

        public async Task<AuthorDto?> GetAuthorByIdAsync(int id)
        {
            var author = await _context.Authors
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.AuthorId == id);

            if (author == null) return null;

            return new AuthorDto
            {
                AuthorId = author.AuthorId,
                FirstName = author.FirstName,
                LastName = author.LastName,
                Country = author.Country,
                BooksCount = author.Books.Count
            };
        }

        public async Task<AuthorDto> CreateAuthorAsync(CreateAuthorDto dto)
        {
            var author = new Author
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Country = dto.Country
            };

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            return await GetAuthorByIdAsync(author.AuthorId)
                   ?? throw new Exception("Ошибка при создании автора");
        }

        public async Task<AuthorDto> UpdateAuthorAsync(int id, CreateAuthorDto dto)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
                throw new NotFoundException($"Автор с ID {id} не найден");

            author.FirstName = dto.FirstName;
            author.LastName = dto.LastName;
            author.Country = dto.Country;

            await _context.SaveChangesAsync();

            return await GetAuthorByIdAsync(id)!;
        }

        public async Task DeleteAuthorAsync(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
                throw new NotFoundException($"Автор с ID {id} не найден");

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
        }
    }
}