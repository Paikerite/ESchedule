using ESchedule.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ESchedule.Models
{
    public class EditAddLessonModel
    {
        [Key]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Необхідна назва урока")]
        public string NameLesson { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Необхідний опис урока")]
        public string DescriptionLesson { get; set; }

        [DataType(DataType.Time)]
        [Required(ErrorMessage = "Необхідно назначити початок урока")]
        public DateTime BeginTime { get; set; }

        [DataType(DataType.Time)]
        [Required(ErrorMessage = "Необхідно назначити кінець урока")]
        public DateTime EndTime { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Необхідно назначити день проведення урока")]
        public DateTime DayTime { get; set; }

        [Required(ErrorMessage = "Необхідно обрати кольор уроку")]
        public string ColorCard { get; set; }

        [DataType(DataType.Date)]
        public DateTime Created { get; set; }

        [CannotBeEmptyList(ErrorMessage = "Необхідно обрати курси, яким буде відображатися даний урок")]
        [Required(ErrorMessage = "Необхідно обрати курси, яким буде відображатися даний урок")]
        public List<int>? SelectedId { get;set; }
    }
}
