using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ESchedule.Data;
using ESchedule.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using ESchedule.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;

namespace ESchedule.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly EScheduleDbContext _context;
        //private readonly IEmailSender emailSender;

        public UserController(EScheduleDbContext context)
        {
            _context = context;
        }

        // GET: User
        //public async Task<IActionResult> Index()
        //{
        //      return _context.Users != null ? 
        //                  View(await _context.Users.ToListAsync()) :
        //                  Problem("Entity set 'EScheduleDbContext.Users'  is null.");
        //}

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var userAccountViewModel = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userAccountViewModel == null)
            {
                return NotFound();
            }

            return View(userAccountViewModel);
        }

        // GET: User/Login
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login([Bind("Email,Password")] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
                if (user != null)
                {
                    await Authenticate(model.Email, user.Role); // аутентификация

                    return RedirectToAction(nameof(ScheduleController.Index), "Schedule");
                }
                ModelState.AddModelError("", "Невірний логін і(або) пароль");
            }
            return View(model);
        }

        // GET: User/Create
        [AllowAnonymous]
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Create([Bind("Id,Name,SurName,PatronymicName,ProfilePicture,Role,Email,Password")] UserAccountViewModel userAccountViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.AnyAsync(a => a.Email == userAccountViewModel.Email);
                if (user == false)
                {
                    _context.Add(userAccountViewModel);
                    await _context.SaveChangesAsync();
                    await Authenticate(userAccountViewModel.Email, userAccountViewModel.Role);

                    return RedirectToAction(nameof(ScheduleController.Index), "Schedule");
                }
                else
                {
                    ModelState.AddModelError("", "Акаунт з цією поштою вже існує");
                }
            }
            return View(userAccountViewModel);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var userAccountViewModel = await _context.Users.FindAsync(id);
            if (userAccountViewModel == null)
            {
                return NotFound();
            }
            return View(userAccountViewModel);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,SurName,PatronymicName,ProfilePicture,Role,Email,Password")] UserAccountViewModel userAccountViewModel)
        {
            if (id != userAccountViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userAccountViewModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserAccountViewModelExists(userAccountViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ScheduleController.Index), "Schedule");
            }
            return View(userAccountViewModel);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var userAccountViewModel = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userAccountViewModel == null)
            {
                return NotFound();
            }

            return View(userAccountViewModel);
        }

        //GET: User/NotHaveRights
        public IActionResult NotHaveRights()
        {
            return View();
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'EScheduleDbContext.Users'  is null.");
            }
            var userAccountViewModel = await _context.Users.FindAsync(id);
            if (userAccountViewModel != null)
            {
                _context.Users.Remove(userAccountViewModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ScheduleController.Index), "Schedule");
        }

        private bool UserAccountViewModelExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [AllowAnonymous]
        private async Task Authenticate(string userName, Roles roles)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, roles.ToString())
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(ScheduleController.Index), "Schedule");
        }
    }
}
