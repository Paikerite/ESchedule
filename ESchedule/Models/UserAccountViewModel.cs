using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESchedule.Models
{
    public class UserAccountViewModel
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string SurName { get; set; }
        public string PatronymicName { get; set; }
        public string ProfilePicture { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public List<ClassViewModel> Classes { get; set; } = new();
    }
}
