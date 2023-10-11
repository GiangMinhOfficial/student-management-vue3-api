using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementApi.Data;
using StudentManagementApi.Models;

namespace StudentManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly StudentManagementContext _context;

        public DepartmentsController(StudentManagementContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartment()
        {
            return await _context.Department.ToListAsync();
        }

        [HttpGet("{departmentId}")]
        public async Task<ActionResult<Department>> GetDepartment(int departmentId)
        {
            var department = await _context.Department.FindAsync(departmentId);

            if (department == null)
            {
                return NotFound();
            }

            return department;
        }

        [HttpPost]
        public async Task<ActionResult<Department>> PostDepartment(DepartmentDTO departmentDTO)
        {

            if (departmentDTO == null || string.IsNullOrEmpty(departmentDTO.DepartmentName))
            {
                return BadRequest();
            }

            var x = _context.Department.Select(d => d.DepartmentName).ToList();

            if (_context.Department.Select(d => d.DepartmentName).Where(name => name.Equals(departmentDTO.DepartmentName)).Count() > 0)
            {
                return BadRequest("Tên khoa đã tồn tại");
            }

            Department department = new Department
            {
                DepartmentName = departmentDTO.DepartmentName
            };

            _context.Department.Add(department);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDepartment), new { id = department.DepartmentId }, department);
        }
    }
}
