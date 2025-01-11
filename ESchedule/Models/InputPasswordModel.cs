using System.ComponentModel.DataAnnotations;

namespace ESchedule.Models
{
    public class InputPasswordModel
    {
        //For delete confirmation
        public int Id { get; set; }
        public string? Name { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
