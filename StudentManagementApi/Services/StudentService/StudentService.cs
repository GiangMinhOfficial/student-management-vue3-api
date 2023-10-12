using Microsoft.EntityFrameworkCore;
using StudentManagementApi.Data;
using StudentManagementApi.Models;

namespace StudentManagementApi.Services.StudentService
{
    public class StudentService : IStudentService
    {
        private readonly StudentManagementContext _context;

        public StudentService(StudentManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Student>> DeleteStudent(int id)
        {
            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return null;
            }

            _context.Student.Remove(student);
            await _context.SaveChangesAsync();

            return await _context.Student.ToListAsync();
        }

        public async Task<Student> GetStudent(int id)
        {
            var student = await _context.Student.FindAsync(id);

            if (student == null)
            {
                return null;
            }

            return student;
        }

        public async Task<IQueryable<Student>> GetStudents(string? search, string? fromDate, string? toDate, bool? gender, int? departmentId,
            string? order = "", int page = 1, int pageSize = 4)
        {
            var students = _context.Student
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

            if (gender.HasValue)
            {
                students = students.Where(s => s.Gender == gender.Value);
            }

            if (departmentId.HasValue)
            {
                students = students.Where(s => s.Department.DepartmentId == departmentId.Value);
            }
            #endregion

            #region Order
            switch (order)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.Name);
                    break;
                case "student_id_asc":
                    students = students.OrderBy(s => s.Id);
                    break;
                case "student_id_desc":
                    students = students.OrderByDescending(s => s.Id);
                    break;
                default:
                    students = students.OrderBy(s => s.Name);
                    break;
            }
            #endregion

            return students;
        }

        public async Task<Student> PostStudent(StudentDTO studentDTO)
        {
            if (studentDTO == null)
            {
                return null;
            }

            Student student = new Student
            {
                Name = studentDTO.Name,
                Birthdate = studentDTO.Birthdate,
                Gender = studentDTO.Gender,
                DepartmentId = studentDTO.DepartmentId,
            };

            _context.Student.Add(student);
            await _context.SaveChangesAsync();

            return student;
        }

        public async Task<IEnumerable<Student>> PutStudent(int id, Student student)
        {
            if (id != student.Id)
            {
                return null;
            }

            _context.Entry(student).State = EntityState.Modified;
            //_context.ChangeTracker.DetectChanges();
            //Console.WriteLine(_context.ChangeTracker.DebugView.LongView);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return await _context.Student.ToListAsync();
        }

        private bool StudentExists(int id)
        {
            return (_context.Student?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
