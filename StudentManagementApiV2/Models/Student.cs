﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementApi.Models
{
    public class Student
    {
        [Key]
        public Guid Id { get; set; }
        public string StudentCode { get; set; }
        public string Name { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }
        public bool Gender { get; set; }
        public int DepartmentId { get; set; }

        public Department Department { get; set; }
    }

    public class StudentDTO
    {
        public string StudentCode { get; set; }
        public string Name { get; set; }
        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }
        public bool Gender { get; set; }
        public int DepartmentId { get; set; }
    }
}
