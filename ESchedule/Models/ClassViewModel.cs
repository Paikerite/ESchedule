using ESchedule.Data;
using System.ComponentModel.DataAnnotations;

namespace ESchedule.Models
{
    public class ClassViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Необхідне назва курсу")]
        [MaxLength(256, ErrorMessage = "Максимальна кількість символів 256")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Необхідне роз'яснення або мета курсу")]
        public string Description { get; set; }
        public string PrimaryColor { get; set; }
        public int IdUserAdmin { get; set; }

        [Required(ErrorMessage = "Необхідний код для приєднання до курсу")]
        [MinLength(6, ErrorMessage = "Мінімальна необхідна кількість символів - 6")]
        [MaxLength(256, ErrorMessage = "Максимальна кількість символів 256")]
        public string CodeToJoin { get; set; }

        public List<ApplicationUser> UsersAccount { get; set; } = new();
        public List<LessonViewModel> Lessons { get; set; } = new();
    }

}
