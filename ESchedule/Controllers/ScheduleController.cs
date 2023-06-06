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

namespace ESchedule.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly EScheduleDbContext _context;

        public ScheduleController(EScheduleDbContext context)
        {
            _context = context;
        }

        // GET: Schedule
        public async Task<IActionResult> Index()
        {
            var currectDate = DateTime.Now;
            var lessons = await _context.Lessons
                .Where(m=>m.DayTime.Month == currectDate.Month && m.DayTime.Year == currectDate.Year)
                                                .OrderBy(a=>a.BeginTime.Hour)
                                                .ToListAsync();

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
            var lessons = await _context.Lessons
                                .OrderByDescending(a => a.Created)
                                .ToListAsync();

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Calendar(DateTime dateTimeByUser)
        {
            CultureInfo cultureUa = new CultureInfo("uk-UA");
            ViewData["UserNameMonth"] = dateTimeByUser.ToString("MMMM", cultureUa);
            ViewData["UserMonth"] = dateTimeByUser.Month;
            ViewData["UserYear"] = dateTimeByUser.Year;

            var lessons = await _context.Lessons
                .Where(m => m.DayTime.Month == dateTimeByUser.Month && m.DayTime.Year == dateTimeByUser.Year)
                                    .OrderBy(a => a.BeginTime.Hour)
                                    .ToListAsync();

            if (lessons != null)
            {
                return PartialView("_Calendar", lessons);
            }
            else
            {
                return Problem("Entity set 'EScheduleDbContext.Lessons' is null.");
            }
        }

        // GET: Schedule/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Lessons == null)
            {
                return NotFound();
            }

            var lessonViewModel = await _context.Lessons
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lessonViewModel == null)
            {
                return NotFound();
            }

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
                _context.Add(lessonViewModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lessonViewModel);
        }

        // GET: Schedule/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Lessons == null)
            {
                return NotFound();
            }

            var lessonViewModel = await _context.Lessons.FindAsync(id);
            if (lessonViewModel == null)
            {
                return NotFound();
            }
            return View(lessonViewModel);
        }

        // POST: Schedule/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameLesson,DescriptionLesson,BeginTime,EndTime,Created,DayTime,ColorCard")] LessonViewModel lessonViewModel)
        {
            if (id != lessonViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lessonViewModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LessonViewModelExists(lessonViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(lessonViewModel);
        }

        // GET: Schedule/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Lessons == null)
            {
                return NotFound();
            }

            var lessonViewModel = await _context.Lessons
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lessonViewModel == null)
            {
                return NotFound();
            }

            return View(lessonViewModel);
        }

        // POST: Schedule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Lessons == null)
            {
                return Problem("Entity set 'EScheduleDbContext.Lessons'  is null.");
            }
            var lessonViewModel = await _context.Lessons.FindAsync(id);
            if (lessonViewModel != null)
            {
                _context.Lessons.Remove(lessonViewModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LessonViewModelExists(int id)
        {
          return (_context.Lessons?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
