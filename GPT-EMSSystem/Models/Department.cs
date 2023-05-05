using System.ComponentModel.DataAnnotations;

namespace GPT_EMSSystem.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }

}
