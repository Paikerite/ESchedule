using System.ComponentModel.DataAnnotations;

namespace ESchedule.Models
{
    public class JoinClassModel
    {
        [Required(ErrorMessage ="Необхідний код для приєднання до курсу")]
        [MinLength(6, ErrorMessage ="Мінімальна необхідна кількість символів - 6")]
        public string JoinCode { get; set; }

        public string? UserName { get; set; }
    }
}
