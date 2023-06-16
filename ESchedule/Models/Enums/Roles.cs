using System.ComponentModel.DataAnnotations;

namespace ESchedule.Models.Enums
{
    public enum Roles
    {
        [Display(Name = "Вчитель")]
        Teacher,
        [Display(Name = "Студент")]
        Student,
        [Display(Name = "Адмін")]
        SuperAdmin
    }
}
