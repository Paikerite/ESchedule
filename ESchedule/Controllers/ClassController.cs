using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ESchedule.Data;
using ESchedule.Models;
using ESchedule.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ESchedule.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ESchedule.Controllers
{
    [Authorize]
    public class ClassController : Controller
    {
        private readonly IClassService classService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILessonService lessonService;

        public ClassController(IClassService classService, ILessonService lessonService, UserManager<ApplicationUser> userManager)
        {
            this.classService = classService;
            this.lessonService = lessonService;
            this.userManager = userManager;
        }

        // GET: Class
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var classes = Enumerable.Empty<ClassViewModel>();
            var currctUserName = User.Identity.Name;

            if (currctUserName != null)
            {
                classes = await classService.GetClassesByUserName(currctUserName);
            }

            if (classes != null)
            {
                return View(classes);
            }
            else
            {
                return Problem("Entity set 'EScheduleDbContext.Classes'  is null.");
            }
        }

        // GET: Class/programming
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string searchName)
        {
            var classes = Enumerable.Empty<ClassViewModel>();
            var currctUserName = User.Identity.Name;

            if (currctUserName != null)
            {
                if (!string.IsNullOrWhiteSpace(searchName))
                {
                    classes = await classService.GetClassesBySearchName(searchName, currctUserName);
                }
                else
                {
                    classes = await classService.GetClassesByUserName(currctUserName);
                }
            }

            if (classes != null)
            {
                return View(classes);
            }
            else
            {
                return Problem("Entity set 'EScheduleDbContext.Classes'  is null.");
            }
        }

        // GET: Class/Details/5
        [Authorize(Roles = "Student,Teacher")]
        public async Task<IActionResult> Details(int id)
        {
            var classViewModel = await classService.GetClass(id);
            if (classViewModel == null)
            {
                return NotFound();
            }

            return View(classViewModel);
        }

        //GET: Class/JoinClass
        [Authorize(Roles = "Student,Teacher")]
        public IActionResult JoinClass()
        {
            return View();
        }

        //POST: Class/JoinClass
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Student,Teacher")]
        public async Task<IActionResult> JoinClass([Bind("JoinCode")] JoinClassModel joinClass)
        {
            if (ModelState.IsValid)
            {
                joinClass.UserName = User.Identity.Name;
                var Class = await classService.GetClassByCodeJoin(joinClass.JoinCode);

                if (Class != null)
                {
                    var currectUser = await userManager.FindByNameAsync(joinClass.UserName);
                    if (currectUser is null)
                    {
                        ModelState.AddModelError("", "Не вдалося приєднати вас до класу");
                        return View(joinClass);
                    }

                    if (!Class.UsersAccount.Contains(currectUser))
                    {
                        await classService.AddUserToClassByCode(joinClass);
                        return RedirectToAction(nameof(Index));
                    }
                    else ModelState.AddModelError("", "Не вдалося записатися на курс. Ви вже приєднані до цього курсу");
                }
                else ModelState.AddModelError("", "Не вдалося записатися на курс. Перевірьте код");

            }
            return View(joinClass);
        }

        // GET: Class/Create
        [Authorize(Roles = "Teacher")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Class/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,IdUserAdmin,CodeToJoin,PrimaryColor")] ClassViewModel classViewModel)
        {
            if (ModelState.IsValid)
            {
                var isExistClass = await classService.GetClassByCodeJoin(classViewModel.CodeToJoin);

                if (isExistClass == null)
                {
                    var JoinClass = new JoinClassModel() { JoinCode = classViewModel.CodeToJoin, UserName = User.Identity.Name };
                    //var currectUser = await userService.GetUserByEmail(JoinClass.UserName);
                    var currectUserId = userManager.GetUserId(User);
                    if (currectUserId is null)
                    {
                        ModelState.AddModelError("", "Не вдалося знайти вас Id");
                        return View(classViewModel);
                    }
                    classViewModel.IdUserAdmin = int.Parse(currectUserId);

                    await classService.PostClass(classViewModel);
                    await classService.AddUserToClassByCode(JoinClass);

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Курс з таким кодом вже існує");
                }
            }
            return View(classViewModel);
        }

        // GET: Class/Edit/5
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Edit(int id)
        {
            var classViewModel = await classService.GetClass(id);

            return View(classViewModel);
        }

        // POST: Class/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,IdUserAdmin,CodeToJoin,PrimaryColor")] ClassViewModel classViewModel)
        {
            if (ModelState.IsValid)
            {
                await classService.UpdateClass(id, classViewModel);

                return RedirectToAction(nameof(Index));
            }
            return View(classViewModel);
        }

        // GET: Class/Delete/5
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Delete(int id)
        {
            var classViewModel = await classService.GetClass(id);

            return View(classViewModel);
        }

        // POST: Class/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var classViewModel = await classService.GetClass(id);

            await classService.DeleteClass(id);

            foreach (var item in classViewModel.Lessons)
            {
                if (item.Classes.Count() != 0)
                {
                    await lessonService.DeleteLesson(item.Id);
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
