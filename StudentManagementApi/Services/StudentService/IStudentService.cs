using StudentManagementApi.Models;

namespace StudentManagementApi.Services.StudentService
{
    public interface IStudentService
    {
        Task<IQueryable<Student>> GetStudents(string? search, string? fromDate, string? toDate, string? order = "", int page = 1, int pageSize = 4);
        Task<Student> GetStudent(int id);
        Task<IEnumerable<Student>> PutStudent(int id, Student student);
        Task<Student> PostStudent(StudentDTO studentDTO);
        Task<IEnumerable<Student>> DeleteStudent(int id);
    }
}
