using API1.DTOs.Author;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API1.Exceptions;         
using API1.Services.Interfaces;
namespace API1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAll()
        {
            var authors = await _authorService.GetAllAuthorsAsync();
            return Ok(authors);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthorDto>> GetById(int id)
        {
            try
            {
                var author = await _authorService.GetAuthorByIdAsync(id);
                return Ok(author);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<AuthorDto>> Create(CreateAuthorDto dto)
        {
            try
            {
                var newAuthor = await _authorService.CreateAuthorAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = newAuthor.AuthorId }, newAuthor);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, CreateAuthorDto dto)
        {
            try
            {
                await _authorService.UpdateAuthorAsync(id, dto);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _authorService.DeleteAuthorAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}


