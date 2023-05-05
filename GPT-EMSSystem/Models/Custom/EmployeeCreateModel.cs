using System.ComponentModel.DataAnnotations;

namespace GPT_EMSSystem.Models.Custom
{
    public class EmployeeCreateModel
    {
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
        public string? Phone { get; set; }

        [Required]
        [StringLength(50)]
        public string Role { get; set; }

        [Required]
        public int DepartmentId { get; set; }
    }
}
