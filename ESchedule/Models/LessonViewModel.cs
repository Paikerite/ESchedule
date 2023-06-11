using System.ComponentModel.DataAnnotations;

namespace ESchedule.Models
{
    public class LessonViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage ="Необхідна назва урока")]
        public string NameLesson { get; set; }

        public string DescriptionLesson { get; set; }

        [DataType(DataType.Time)]
        public DateTime BeginTime { get; set; }
        [DataType(DataType.Time)]
        public DateTime EndTime { get; set; }
        [DataType(DataType.Date)]
        public DateTime DayTime { get; set; }
        [DataType(DataType.Date)]
        public DateTime Created { get; set; }

        public string ColorCard { get; set; }
        
        public List<ClassViewModel> Classes { get; set; } = new();
    }
}
