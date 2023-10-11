using Microsoft.EntityFrameworkCore;
using StudentManagementApi.Models;

namespace StudentManagementApiV2.Data
{
    public class StudentManagementApiContext : DbContext
    {
        public StudentManagementApiContext(DbContextOptions<StudentManagementApiContext> options)
            : base(options)
        {
        }

        public DbSet<StudentManagementApi.Models.Student> Students { get; set; } = default!;
        public DbSet<Department> Departments { get; set; } = default!;
    }
}
