using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ESchedule.Data;
using ESchedule.Models;
using System.Globalization;
using ESchedule.Services.Interfaces;
using ESchedule.Services;
using Microsoft.AspNetCore.Authorization;

namespace ESchedule.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly EScheduleDbContext _context;
        private readonly ILessonService lessonService;

        public ScheduleController(EScheduleDbContext context, ILessonService lessonService)
        {
            _context = context;
            this.lessonService = lessonService;
        }

        // GET: Schedule
        public async Task<IActionResult> Index()
        {   
            var currectDate = DateTime.Now; //Normal date

            var lessons = await lessonService.GetLessonsByDate(currectDate);
            ViewBag.Date = currectDate;

            if (lessons != null)
            {
                return View(lessons);
            }
            else
            {
                return Problem("Entity set 'EScheduleDbContext.Lessons' is null.");
            }
        }

        // POST: Schedule/{datebyuser}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(DateTime datebyuser)
        {   
            var lessons = await lessonService.GetLessonsByDate(datebyuser);
            ViewBag.Date = datebyuser;

            if (lessons != null)
            {
                return View(lessons);
            }
            else
            {
                return Problem("Entity set 'EScheduleDbContext.Lessons' is null.");
            }
        }

        // GET: Schedule/IndexShowLessonsList
        public async Task<IActionResult> IndexShowLessonsList()
        {
            var lessons = await lessonService.GetLessons();

            if (lessons != null)
            {
                return View(lessons);
            }
            else
            {
                return Problem("Entity set 'EScheduleDbContext.Lessons' is null.");
            }
        }

        //POST: Schedule/Calendar
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Calendar(DateTime dateTimeByUser)
        //{
        //    CultureInfo cultureUa = new CultureInfo("uk-UA");
        //    ViewData["UserNameMonth"] = dateTimeByUser.ToString("MMMM", cultureUa);
        //    ViewData["UserMonth"] = dateTimeByUser.Month;
        //    ViewData["UserYear"] = dateTimeByUser.Year;

        //    var lessons = await _context.Lessons
        //        .Where(m => m.DayTime.Month == dateTimeByUser.Month && m.DayTime.Year == dateTimeByUser.Year)
        //                            .OrderBy(a => a.BeginTime.Hour)
        //                            .ToListAsync();

        //    if (lessons != null)
        //    {
        //        return PartialView("_Calendar", lessons);
        //    }
        //    else
        //    {
        //        return Problem("Entity set 'EScheduleDbContext.Lessons' is null.");
        //    }
        //}

        // GET: Schedule/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var lessonViewModel = await lessonService.GetLesson(id);

            return View(lessonViewModel);
        }

        // GET: Schedule/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Schedule/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NameLesson,DescriptionLesson,BeginTime,EndTime,Created,DayTime,ColorCard")] LessonViewModel lessonViewModel)
        {
            if (ModelState.IsValid)
            {
                lessonViewModel.Created = DateTime.Now.Date;

                lessonViewModel = await lessonService.AddLesson(lessonViewModel);
            }
            return RedirectToAction(nameof(IndexShowLessonsList));
        }

        // GET: Schedule/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var lessonViewModel = await lessonService.GetLesson(id);
            return View(lessonViewModel);
        }

        // POST: Schedule/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameLesson,DescriptionLesson,BeginTime,EndTime,Created,DayTime,ColorCard")] LessonViewModel lessonViewModel)
        {
            lessonViewModel = await lessonService.UpdateLesson(id, lessonViewModel);

            //return View(lessonViewModel);
            return RedirectToAction(nameof(IndexShowLessonsList));
        }

        // GET: Schedule/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var lessonViewModel = await lessonService.GetLesson(id);

            return View(lessonViewModel);
        }

        // POST: Schedule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await lessonService.DeleteLesson(id);

            return RedirectToAction(nameof(IndexShowLessonsList));
        }

        private bool LessonViewModelExists(int id)
        {
          return (_context.Lessons?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
