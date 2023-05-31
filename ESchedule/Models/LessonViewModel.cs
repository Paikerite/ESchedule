using System.ComponentModel.DataAnnotations;

namespace ESchedule.Models
{
    public class LessonViewModel
    {
        [Key]
        public int Id { get; set; }

        public string NameLesson { get; set; }
        public string DescriptionLesson { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime Created { get; set; }
        
        public List<ClassViewModel> Classes { get; set; } = new();
    }
}
