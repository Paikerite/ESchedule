using ESchedule.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ESchedule.Models
{
    public class MultipleSelectModel
    {
        public IEnumerable<int>? SelectedId { get;set; }

        [CannotBeEmptyList(ErrorMessage = "Необхідно обрати курси, яким буде відображатися даний урок")]
        public SelectList? SelectedClasses { set; get; }
    }
}
