using System.ComponentModel.DataAnnotations;

namespace ESchedule.Models
{
    public class AddClassesToLessonModel
    {
        public int IdLesson { get; set; }

        [Required]
        public IEnumerable<int> IdsOfClasses { get; set; }
    }
}
