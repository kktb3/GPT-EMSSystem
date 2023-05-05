using System.ComponentModel.DataAnnotations;

namespace GPT_EMSSystem.Models.Custom
{
    public class AttendanceInputModel
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public bool IsPresent { get; set; }
    }
}
