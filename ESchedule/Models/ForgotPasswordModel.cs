using System.ComponentModel.DataAnnotations;

namespace ESchedule.Models
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
    }
}
