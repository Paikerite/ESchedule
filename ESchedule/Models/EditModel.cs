using ESchedule.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ESchedule.Models
{
    public class EditModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Не вказано ім'я")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Не вказана фамілія")]
        public string? SurName { get; set; }

        [Required(ErrorMessage = "Не вказано прізвище по батькові")]
        public string? PatronymicName { get; set; }

        public string? ProfilePicture { get; set; }

        [Required(ErrorMessage = "Будь ласка, оберіть вашу роль в системі")]
        [EnumDataType(typeof(Roles), ErrorMessage = "Будь ласка, оберіть вашу роль в системі з переліку")]
        public Roles Role { get; set; }

        [Required(ErrorMessage = "Не вказан пароль")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Не вказан Email")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
    }
}
