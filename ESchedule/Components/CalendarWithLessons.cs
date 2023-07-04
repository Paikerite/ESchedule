using ESchedule.Models;
using ESchedule.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ESchedule.Components
{
    public class CalendarWithLessons : ViewComponent
    {
        private readonly ILessonService lessonService;

        public CalendarWithLessons(ILessonService lessonService)
        {
            this.lessonService = lessonService;
        }

        public async Task<IViewComponentResult> InvokeAsync(IEnumerable<LessonViewModel> lessons, DateTime date)
        {
            //var lessons = await lessonService.GetLessonsByDate(date);
            //ViewData["DataByUser"] = date;
            ViewBag.Data = date;
            if (lessons != null)
            {
                return View(lessons);
            }
            else
            {
                //ViewData["IsLessonNull"] = true;
                return View();
            }
        }
    }
}
