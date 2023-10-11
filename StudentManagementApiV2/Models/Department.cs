namespace StudentManagementApi.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        public ICollection<Student> Students { get; set; }
    }

    public class DepartmentDTO
    {
        public string DepartmentName { get; set; }
    }
}
