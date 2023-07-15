using Microsoft.AspNetCore.Mvc;
using ESchedule.Models;
using ESchedule.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Channels;
using Microsoft.Extensions.Logging.EventSource;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace ESchedule.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly ILessonService lessonService;
        private readonly IClassService classService;
        private readonly IUserService userService;

        public ScheduleController(ILessonService lessonService, IClassService classService, IUserService userService)
        {
            this.lessonService = lessonService;
            this.classService = classService;
            this.userService = userService;
        }

        // GET: Schedule
        public async Task<IActionResult> Index()
        {
            var currectDate = DateTime.Now; //Normal date

            var lessons = await lessonService.GetLessonsByDateAndName(currectDate, User.Identity.Name);
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
            var lessons = await lessonService.GetLessonsByDateAndName(datebyuser, User.Identity.Name);
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
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> IndexShowLessonsList()
        {
            var lessons = await lessonService.GetLessonsByName(User.Identity.Name);

            if (lessons != null)
            {
                return View(lessons);
            }
            else
            {
                return Problem("Entity set 'EScheduleDbContext.Lessons' is null.");
            }
        }

        // GET: Schedule/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var lessonViewModel = await lessonService.GetLesson((int)id);

            return View(lessonViewModel);
        }

        // GET: Schedule/Create
        [Authorize(Roles = "Teacher")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Schedule/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Create([Bind("NameLesson,DescriptionLesson,BeginTime,EndTime,DayTime,ColorCard,SelectedId")] EditAddLessonModel lessonModel)
        {
            LessonViewModel lessonViewModel = new()
            {
                NameLesson = lessonModel.NameLesson,
                DescriptionLesson = lessonModel.DescriptionLesson,
                BeginTime = lessonModel.BeginTime,
                EndTime = lessonModel.EndTime,
                DayTime = lessonModel.DayTime,
                ColorCard = lessonModel.ColorCard,
                Created = DateTime.Today
            };

            if (ModelState.IsValid)
            {
                var result = await lessonService.AddLesson(lessonViewModel);
                if (result != null)
                {
                    await lessonService.AddClassesToLesson(new AddClassesToLessonModel() { IdLesson = result.Id, IdsOfClasses = lessonModel.SelectedId});
                    return RedirectToAction(nameof(Index));
                }
            }

            //  fail: Microsoft.EntityFrameworkCore.Update[10000]
            //An exception occurred in the database while saving changes for context type 'ESchedule.Data.EScheduleDbContext'.
            //Microsoft.EntityFrameworkCore.DbUpdateException: An error occurred while saving the entity changes.See the inner exception for details.
            // --->Microsoft.Data.SqlClient.SqlException(0x80131904): Невозможно вставить явное значение для столбца идентификаторов в таблице "Classes", когда параметр IDENTITY_INSERT имеет значение OFF.
            return View(lessonModel);
        }

        // GET: Schedule/Edit/5
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Edit(int? id, string userName)
        {

            var user = await userService.GetUserByEmail(userName);

            if (user == null)
            {
                return BadRequest();
            }
            
            var lessonViewModel = await lessonService.GetLesson((int)id);
            if (lessonViewModel.Classes.Any(a => a.IdUserAdmin == user.Id))
            {
                EditAddLessonModel model = new()
                {
                    Id = lessonViewModel.Id,
                    NameLesson = lessonViewModel.NameLesson,
                    DescriptionLesson = lessonViewModel.DescriptionLesson,
                    BeginTime = lessonViewModel.BeginTime,
                    EndTime = lessonViewModel.EndTime,
                    DayTime = lessonViewModel.DayTime,
                    ColorCard = lessonViewModel.ColorCard,
                    Created = lessonViewModel.Created,
                    SelectedId = lessonViewModel.Classes.Select(c => c.Id).ToList()
                };
                return View(model);
            }
            else 
            {
                return RedirectToAction("NotHaveRights", "User");
            }

        }

        // POST: Schedule/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameLesson,DescriptionLesson,BeginTime,EndTime,Created,DayTime,ColorCard,SelectedId")] EditAddLessonModel lessonModel)
        {
            if (ModelState.IsValid)
            {
                LessonViewModel lessonViewModel = new()
                {
                    Id = lessonModel.Id,
                    NameLesson = lessonModel.NameLesson,
                    DescriptionLesson = lessonModel.DescriptionLesson,
                    BeginTime = lessonModel.BeginTime,
                    EndTime = lessonModel.EndTime,
                    DayTime = lessonModel.DayTime,
                    ColorCard = lessonModel.ColorCard,
                    Created = DateTime.Today,
                };

                lessonViewModel = await lessonService.UpdateLesson(id, lessonViewModel);

                if (lessonViewModel != null)
                {
                    await lessonService.AddClassesToLesson(new AddClassesToLessonModel() { IdLesson = lessonViewModel.Id, IdsOfClasses = lessonModel.SelectedId });
                    return RedirectToAction(nameof(IndexShowLessonsList));
                }
            }
            return View(lessonModel);
        }

        // GET: Schedule/Delete/5
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Delete(int? id)
        {
            var lessonViewModel = await lessonService.GetLesson((int)id);

            return View(lessonViewModel);
        }

        // POST: Schedule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await lessonService.DeleteLesson(id);

            return RedirectToAction(nameof(IndexShowLessonsList));
        }
    }
}
