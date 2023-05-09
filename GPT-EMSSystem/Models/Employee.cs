using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GPT_EMSSystem.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(50)]
        public string Role { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        public Department Department { get; set; }

        //public ICollection<Attendance> AttendanceRecords { get; set; }

        //public ICollection<Payroll> Payrolls { get; set; }

        //public ICollection<Performance> Performances { get; set; }

        //public ICollection<Training> Trainings { get; set; }

        //public ICollection<Leave> Leaves { get; set; }
    }

}
