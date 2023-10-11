using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementApi.Models;
using StudentManagementApiV2.Data;

namespace StudentManagementApiV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly StudentManagementApiContext _context;

        public StudentsController(StudentManagementApiContext context)
        {
            _context = context;
        }

        // GET: api/Students
        [HttpGet]
        public IActionResult GetStudent(string? search, string? fromDate, string? toDate, string? order = "", int page = 1, int pageSize = 4)
        {
            var students = _context.Students
                .AsNoTracking()
                //.Include(s => s.Department)
                .Select(s => new Student
                {
                    Id = s.Id,
                    Name = s.Name,
                    Birthdate = s.Birthdate,
                    Gender = s.Gender,
                    Department = new Department
                    {
                        DepartmentId = s.Department.DepartmentId,
                        DepartmentName = s.Department.DepartmentName
                    }
                })
                .AsQueryable();

            #region Filter
            if (!string.IsNullOrEmpty(search))
            {
                students = students.Where(s => s.Id.ToString().Contains(search)
                || s.Name.Contains(search));
            }

            if (!string.IsNullOrEmpty(fromDate))
            {
                students = students.Where(s => s.Birthdate >= DateTime.Parse(fromDate));
            }

            if (!string.IsNullOrEmpty(toDate))
            {
                students = students.Where(s => s.Birthdate <= DateTime.Parse(toDate));
            }
            #endregion

            #region Order
            switch (order)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.Name);
                    break;
                default:
                    students = students.OrderBy(s => s.Name);
                    break;
            }
            #endregion

            var result = PaginatedList<Student>.Create(students, page, pageSize);

            return Ok(new
            {
                Students = result,
                TotalPage = result.TotalPage,
            });
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(Guid id)
        {
            if (_context.Students == null)
            {
                return NotFound();
            }
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }

        // PUT: api/Students/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(Guid id, Student student)
        {
            if (id != student.Id)
            {
                return BadRequest();
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Students
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            if (_context.Students == null)
            {
                return Problem("Entity set 'StudentManagementApiContext.Student'  is null.");
            }
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudent", new { id = student.Id }, student);
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            if (_context.Students == null)
            {
                return NotFound();
            }
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentExists(Guid id)
        {
            return (_context.Students?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
