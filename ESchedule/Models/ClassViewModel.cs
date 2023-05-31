using System.ComponentModel.DataAnnotations;

namespace ESchedule.Models
{
    public class ClassViewModel
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
