using Microsoft.AspNetCore.Mvc;
using ESchedule.Models;
using ESchedule.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        [Authorize(Roles = "Teacher")]
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

        // GET: Schedule/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var lessonViewModel = await lessonService.GetLesson(id);

            return View(lessonViewModel);
        }

        // GET: Schedule/Create
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Create()
        {
            //LessonAddEditModel lessonModel = new() { DayTime = DateTime.Now };

            var currectUser = await userService.GetUserByEmail(User.Identity.Name);
            var availableClasses = await classService.GetClassesByAdminId(currectUser.Id);

            var selectList = new SelectList(availableClasses, nameof(ClassViewModel.Id), nameof(ClassViewModel.Name));

            var tuple = new Tuple<LessonViewModel, MultipleSelectModel>(new LessonViewModel() { DayTime=DateTime.Now, ColorCard = "#ffffff"}, new MultipleSelectModel() { SelectedClasses=selectList });

            return View(tuple);
        }

        // POST: Schedule/RemoveCourse/...
        //[HttpPost]
        //[Authorize(Roles = "Teacher")]
        //public IActionResult RemoveCourse(LessonViewModel lessonViewModel, ClassViewModel classViewModel)
        //{
        //    lessonViewModel.Classes.Remove(classViewModel);

        //    return View("Create",lessonViewModel);
        //}

        // POST: Schedule/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Create([Bind("Id,NameLesson,DescriptionLesson,BeginTime,EndTime,DayTime,Created,ColorCard,Classes")] LessonViewModel lessonModel,
                                                [Bind("SelectedId, SelectedClasses")] MultipleSelectModel multipleSelectModel)
        {
            if (ModelState.IsValid)
            {
                //List<ClassViewModel> fullSelectedClasses = new();

                //foreach (var item in lessonModel.SelectedClasses)
                //{
                //    fullSelectedClasses.Add(await classService.GetClass(int.Parse(item.Value)));
                //}

                //LessonViewModel lessonViewModel = new()
                //{
                //    NameLesson = lessonModel.NameLesson,
                //    DescriptionLesson = lessonModel.DescriptionLesson,
                //    BeginTime = lessonModel.BeginTime,
                //    EndTime = lessonModel.EndTime,
                //    DayTime = lessonModel.DayTime,
                //    Created = DateTime.Now,
                //    ColorCard = lessonModel.ColorCard,
                //    Classes=fullSelectedClasses
                //};

                await lessonService.AddLesson(lessonModel);

                return RedirectToAction(nameof(Index));
            }
            return View(lessonModel);
        }

        // GET: Schedule/Edit/5
        [Authorize(Roles = "Teacher")]
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
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameLesson,DescriptionLesson,BeginTime,EndTime,Created,DayTime,ColorCard")] LessonViewModel lessonViewModel)
        {
            if (ModelState.IsValid)
            {
                lessonViewModel = await lessonService.UpdateLesson(id, lessonViewModel);
                return RedirectToAction(nameof(IndexShowLessonsList));
            }
            return View(lessonViewModel);
        }

        // GET: Schedule/Delete/5
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Delete(int id)
        {
            var lessonViewModel = await lessonService.GetLesson(id);

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
