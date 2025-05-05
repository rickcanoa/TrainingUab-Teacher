using AppTrainingBE.Context;
using AppTrainingBETeacher.DTOs;
using AppTrainingBETeacher.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppTrainingBETeacher.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BooksController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetAll()
        {
            var books = await _context.Books
                .Include(b => b.AuthorBooks)
                .Select(b => new BookDto
                {
                    Id = b.BookId,
                    Title = b.Title,
                    AuthorIds = b.AuthorBooks.Select(ab => ab.AuthorId).ToList()
                }).ToListAsync();

            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> Get(int id)
        {
            var book = await _context.Books
                .Include(b => b.AuthorBooks)
                .FirstOrDefaultAsync(b => b.BookId == id);

            if (book == null)
                return NotFound();

            var dto = new BookDto
            {
                Id = book.BookId,
                Title = book.Title,
                AuthorIds = book.AuthorBooks.Select(ab => ab.AuthorId).ToList()
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BookDto dto)
        {
            var book = new Book
            {
                Title = dto.Title,
                AuthorBooks = dto.AuthorIds?.Select(authorId => new AuthorBook
                {
                    AuthorId = authorId
                }).ToList()
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            dto.Id = book.BookId;
            return CreatedAtAction(nameof(Get), new { id = book.BookId }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, BookDto dto)
        {
            if (id != dto.Id)
                return BadRequest();

            var book = await _context.Books
                .Include(b => b.AuthorBooks)
                .FirstOrDefaultAsync(b => b.BookId == id);

            if (book == null)
                return NotFound();

            book.Title = dto.Title;

            // Limpiamos relaciones previas
            book.AuthorBooks.Clear();
            book.AuthorBooks = dto.AuthorIds?.Select(authorId => new AuthorBook
            {
                AuthorId = authorId,
                BookId = id
            }).ToList();

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
                return NotFound();

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
