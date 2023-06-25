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

namespace ESchedule.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class ClassController : Controller
    {
        private readonly IClassService classService;
        private readonly IUserService userService;

        public ClassController(IClassService classService, IUserService userService)
        {
            this.classService = classService;
            this.userService = userService;
        }

        // GET: Class
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            //var classes = await _context.Classes
            //        .Where(c => c.UsersAccount.Any(u => u.Email == currctUserName))
            //        .Include(a => a.Lessons)
            //        .ToListAsync();
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

        // GET: Class/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            //if (id == null || _context.Classes == null)
            //{
            //    return NotFound();
            //}

            //var classViewModel = await _context.Classes
            //    .FirstOrDefaultAsync(m => m.Id == id);
            var classViewModel = await classService.GetClass(id);
            if (classViewModel == null)
            {
                return NotFound();
            }

            return View(classViewModel);
        }

        //GET: Class/JoinClass
        public IActionResult JoinClass()
        {
            return View();
        }

        //POST: Class/JoinClass
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> JoinClass([Bind("JoinCode, UserName")] JoinClassModel joinClass)
        {
            if (ModelState.IsValid)
            {
                joinClass.UserName = User.Identity.Name;
                var Class = await classService.GetClassByCodeJoin(joinClass.JoinCode);

                if (Class != null)
                {
                    var currectUser = await userService.GetUserByEmail(joinClass.UserName);
                    if (!Class.UsersAccount.Contains(currectUser))
                    {
                        //        Class.UsersAccount.Add(currectUser);
                        //        await _context.SaveChangesAsync();
                        //        return RedirectToAction(nameof(Index));
                        await classService.AddUserToClassByCode(joinClass);
                    }
                    else ModelState.AddModelError("", "Не вдалося записатися на курс. Ви вже приєднані до цього курсу");
                }
                else ModelState.AddModelError("", "Не вдалося записатися на курс. Перевірьте код");

        }
            return View(joinClass);
        }

        // GET: Class/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Class/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,IdUserAdmin,CodeToJoin,PrimaryColor")] ClassViewModel classViewModel)
        {
            if (ModelState.IsValid)
            {
                //var currectUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);
                //classViewModel.UsersAccount.Add(currectUser);

                //_context.Add(classViewModel);
                //await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                var isExistClass = await classService.GetClassByCodeJoin(classViewModel.CodeToJoin);
                if (isExistClass == null)
                {
                    var JoinClass = new JoinClassModel() { JoinCode = classViewModel.CodeToJoin, UserName = User.Identity.Name };
                    var currectUser = await userService.GetUserByEmail(JoinClass.UserName);
                    classViewModel.IdUserAdmin = currectUser.Id;

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
        public async Task<IActionResult> Edit(int id)
        {
            //if (id == null || _context.Classes == null)
            //{
            //    return NotFound();
            //}

            //var classViewModel = await _context.Classes.FindAsync(id);
            //if (classViewModel == null)
            //{
            //    return NotFound();
            //}
            var classViewModel = await classService.GetClass(id);

            return View(classViewModel);
        }

        // POST: Class/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,IdUserAdmin,CodeToJoin,PrimaryColor")] ClassViewModel classViewModel)
        {
            //if (id != classViewModel.Id)
            //{
            //    return NotFound();
            //}

            if (ModelState.IsValid)
            {
                //try
                //{
                //    _context.Update(classViewModel);
                //    await _context.SaveChangesAsync();
                //}
                //catch (DbUpdateConcurrencyException)
                //{
                //    if (!ClassViewModelExists(classViewModel.Id))
                //    {
                //        return NotFound();
                //    }
                //    else
                //    {
                //        throw;
                //    }
                //}
                await classService.UpdateClass(id, classViewModel);

                return RedirectToAction(nameof(Index));
            }
            return View(classViewModel);
        }

        // GET: Class/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            //if (id == null || _context.Classes == null)
            //{
            //    return NotFound();
            //}

            //var classViewModel = await _context.Classes
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //if (classViewModel == null)
            //{
            //    return NotFound();
            //}
            var classViewModel = await classService.GetClass(id);

            return View(classViewModel);
        }

        // POST: Class/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //if (_context.Classes == null)
            //{
            //    return Problem("Entity set 'EScheduleDbContext.Classes'  is null.");
            //}
            //var classViewModel = await _context.Classes.FindAsync(id);
            //if (classViewModel != null)
            //{
            //    _context.Classes.Remove(classViewModel);
            //}
            
            //await _context.SaveChangesAsync();
            await classService.DeleteClass(id);

            return RedirectToAction(nameof(Index));
        }

        //private bool ClassViewModelExists(int id)
        //{
        //  return (_context.Classes?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}
