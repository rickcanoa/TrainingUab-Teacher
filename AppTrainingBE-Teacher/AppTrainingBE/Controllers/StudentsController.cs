using AppTrainingBE.Context;
using AppTrainingBETeacher.DTOs;
using AppTrainingBETeacher.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppTrainingBETeacher.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StudentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET api/students
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var students = await _context.Students
                .Include(s => s.StudentCourses)
                    .ThenInclude(sc => sc.Course)
                .ToListAsync();

            var result = students.Select(s => new
            {
                s.Id,
                s.FullName,
                Courses = s.StudentCourses.Select(sc => new { sc.Course.Id, sc.Course.Title })
            });

            return Ok(result);
        }

        // GET api/students/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var student = await _context.Students
                .Include(s => s.StudentCourses)
                    .ThenInclude(sc => sc.Course)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null) return NotFound();

            return Ok(new
            {
                student.Id,
                student.FullName,
                Courses = student.StudentCourses.Select(sc => new { sc.Course.Id, sc.Course.Title })
            });
        }

        // POST api/students
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StudentDto dto)
        {
            // Validar si los CourseIds existen
            var existingCourseIds = await _context.Courses
                .Where(c => dto.CourseIds.Contains(c.Id))
                .Select(c => c.Id)
                .ToListAsync();

            var invalidIds = dto.CourseIds.Except(existingCourseIds).ToList();

            if (invalidIds.Any())
            {
                return BadRequest($"Los siguientes CourseIds no existen: {string.Join(", ", invalidIds)}");
            }

            var student = new Student
            {
                FullName = dto.FullName,
                StudentCourses = dto.CourseIds.Select(cid => new StudentCourse
                {
                    CourseId = cid
                }).ToList()
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = student.Id }, new
            {
                student.Id,
                student.FullName,
                Courses = student.StudentCourses.Select(sc => new { sc.CourseId })
            });
        }

        // PUT api/students/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] StudentDto dto)
        {
            var student = await _context.Students
                .Include(s => s.StudentCourses)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null) return NotFound();

            student.FullName = dto.FullName;

            student.StudentCourses.Clear();
            foreach (var cid in dto.CourseIds)
            {
                student.StudentCourses.Add(new StudentCourse { StudentId = id, CourseId = cid });
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE api/students/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _context.Students
                .Include(s => s.StudentCourses)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null) return NotFound();

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
