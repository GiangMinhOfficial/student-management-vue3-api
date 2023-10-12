using Microsoft.AspNetCore.Mvc;
using StudentManagementApi.Models;
using StudentManagementApi.Services.StudentService;

namespace StudentManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;
        public static int PAGE_SIZE = 4;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // GET: api/Students
        [HttpGet]
        public async Task<IActionResult> GetStudent(string? search, string? fromDate, string? toDate, bool? gender, int? departmentId,
            string? order = "", int page = 1, int pageSize = 4)
        {
            var students = await _studentService.GetStudents(search, fromDate, toDate, gender, departmentId, order, page, pageSize);

            var result = PaginatedList<Student>.Create(students, page, pageSize);

            return Ok(new
            {
                Students = result,
                TotalPage = result.TotalPage,
                TotalStudent = students.Count()
            });
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await _studentService.GetStudent(id);

            if (student == null)
            {
                return NotFound($"Không tìm thấy sinh viên mã {id}");
            }

            return student;
        }

        // PUT: api/Students/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<IEnumerable<Student>>> PutStudent(int id, Student student)
        {
            var students = await _studentService.PutStudent(id, student);

            if (students == null)
            {
                return NotFound($"Không tìm thấy sinh viên mã {id}");
            }

            return Ok(students);
        }

        // POST: api/Students
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(StudentDTO studentDTO)
        {
            var student = await _studentService.PostStudent(studentDTO);

            return CreatedAtAction(nameof(GetStudent), new { id = student.Id });
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var students = await _studentService.DeleteStudent(id);

            if (students == null)
            {
                return NotFound($"Không tìm thấy sinh viên mã {id}");
            }

            return Ok(students);
        }
    }
}
