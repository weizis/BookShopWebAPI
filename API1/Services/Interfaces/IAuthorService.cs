using API1.DTOs.Author;

namespace API1.Services.Interfaces
{
    public interface IAuthorService
    {
        Task<IEnumerable<AuthorDto>> GetAllAuthorsAsync();
        Task<AuthorDto?> GetAuthorByIdAsync(int id);
        Task<AuthorDto> CreateAuthorAsync(CreateAuthorDto dto);
        Task<AuthorDto> UpdateAuthorAsync(int id, CreateAuthorDto dto);
        Task DeleteAuthorAsync(int id);
    }
}