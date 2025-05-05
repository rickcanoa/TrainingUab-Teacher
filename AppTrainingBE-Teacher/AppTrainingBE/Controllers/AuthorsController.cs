using AppTrainingBE.Context;
using AppTrainingBETeacher.DTOs;
using AppTrainingBETeacher.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppTrainingBETeacher.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {

        private readonly AppDbContext _context;

        public AuthorsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAll()
        {
            var authors = await _context.Authors
                .Include(a => a.AuthorBooks)
                .Select(a => new AuthorDto
                {
                    Id = a.AuthorId,
                    Name = a.Name,
                    BookIds = a.AuthorBooks.Select(ab => ab.BookId).ToList()
                }).ToListAsync();

            return Ok(authors);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AuthorDto dto)
        {
            var author = new Author
            {
                Name = dto.Name,
                AuthorBooks = dto.BookIds?.Select(bookId => new AuthorBook
                {
                    BookId = bookId
                }).ToList()
            };

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAll), new { id = author.AuthorId }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, AuthorDto dto)
        {
            var author = await _context.Authors
                .Include(a => a.AuthorBooks)
                .FirstOrDefaultAsync(a => a.AuthorId == id);

            if (author == null) return NotFound();

            author.Name = dto.Name;

            // Actualizamos los libros relacionados
            author.AuthorBooks.Clear();
            author.AuthorBooks = dto.BookIds?.Select(bookId => new AuthorBook
            {
                AuthorId = id,
                BookId = bookId
            }).ToList();

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null) return NotFound();

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
