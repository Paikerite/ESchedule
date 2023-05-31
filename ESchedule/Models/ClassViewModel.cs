using System.ComponentModel.DataAnnotations;

namespace ESchedule.Models
{
    public class ClassViewModel
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public int IdUserAdmin { get; set; }

        public string CodeToJoin { get; set; }

        public List<UserAccountViewModel> UsersAccount { get; set; } = new();
        public List<LessonViewModel> Lessons { get; set; } = new();
    }

}
