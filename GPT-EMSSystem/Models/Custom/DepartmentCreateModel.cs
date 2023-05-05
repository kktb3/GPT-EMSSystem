using System.ComponentModel.DataAnnotations;

namespace GPT_EMSSystem.Models.Custom
{
    public class DepartmentCreateModel
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
