using System.ComponentModel.DataAnnotations;

namespace GPT_EMSSystem.Models.Custom
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(50)]
        public string Password { get; set; }
    }

}
