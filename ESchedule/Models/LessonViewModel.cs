using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public int NameClass { get; set; }
        public int IdUserTeacher { get; set; }

        [ForeignKey("IdUserTeacher")]
        public UserViewModel UserTeacher { get; set; }
    }
}
