
using API1.DTOs.Book;
using API1.Exceptions;
using API1.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetAll()
        {
            var books = await _bookService.GetAllBooksAsync();
            return Ok(books);
        }

        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<BookDto>>> Search(
            [FromQuery] string title)
        {
            var books = await _bookService.SearchBooksAsync(title ?? "");
            return Ok(books);
        }

        [HttpGet("by-author")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetByAuthor(
            [FromQuery] string author)
        {
            var books = await _bookService.GetBooksByAuthorAsync(author ?? "");
            return Ok(books);
        }


        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<BookDto>> GetById(int id)
        {
            try
            {
                var book = await _bookService.GetBookByIdAsync(id);

                if (book == null)
                    return NotFound(new { message = $"Книга с ID {id} не найдена" });

                return Ok(book);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<BookDto>> Create(CreateBookDto dto)
        {
            try
            {
                var newBook = await _bookService.CreateBookAsync(dto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = newBook.BookID },
                    newBook);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, CreateBookDto dto)
        {
            try
            {
                await _bookService.UpdateBookAsync(id, dto);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _bookService.DeleteBookAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
