using ESchedule.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ESchedule.Data
{
    public class ApplicationUser : IdentityUser<int>
    {
        [Required(ErrorMessage = "Не вказано ім'я")]
        [MaxLength(256, ErrorMessage = "Максимальна кількість символів 256")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Не вказана фамілія")]
        [MaxLength(256, ErrorMessage = "Максимальна кількість символів 256")]
        public string SurName { get; set; }
        [Required(ErrorMessage = "Не вказано прізвище по батькові")]
        [MaxLength(256, ErrorMessage = "Максимальна кількість символів 256")]
        public string PatronymicName { get; set; }
        public string? ProfilePicture { get; set; }
        public List<ClassViewModel> Classes { get; set; } = new();
    }
}
