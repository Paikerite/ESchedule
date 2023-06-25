using System.ComponentModel.DataAnnotations;

namespace ESchedule.Models
{
    public class ClassViewModel
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string PrimaryColor { get; set; }
        public int IdUserAdmin { get; set; }

        [Required(ErrorMessage = "Необхідний код для приєднання до курсу")]
        [MinLength(6, ErrorMessage = "Мінімальна необхідна кількість символів - 6")]
        public string CodeToJoin { get; set; }

        public List<UserAccountViewModel> UsersAccount { get; set; } = new();
        public List<LessonViewModel> Lessons { get; set; } = new();
    }

}
