using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ESchedule.Data;
using ESchedule.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using ESchedule.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ESchedule.Services;
using ESchedule.Services.Interfaces;
using System.ComponentModel;

namespace ESchedule.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IEmailService emailService;

        public AccountController(EScheduleDbContext context, IEmailService emailService, UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager)
        {
            this.emailService = emailService;
            userManager = _userManager;
            signInManager = _signInManager;
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
        public async Task<IActionResult> Login([Bind("Email,Password,RememberMe")] LoginModel model)
        {
            //if (ModelState.IsValid)
            //{
            //    var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
            //    if (user != null)
            //    {
            //        if (user.IsConfirmEmail)
            //        {
            //            await Authenticate(model.Email, user.Role); // аутентификация

            //            return RedirectToAction(nameof(ScheduleController.Index), "Schedule");
            //        }
            //        else
            //        {
            //            return RedirectToAction("MustConfirmEmail", new {UserId = user.Id});
            //        }
            //    }
            //    ModelState.AddModelError("", "Невірний логін і(або) пароль");
            //}
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email!, model.Password!, model.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(ScheduleController.Index), "Schedule");
                }

                ModelState.AddModelError("", "Невдала спроба ввійти");
            }
            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> MustConfirmEmail(int UserId)
        {
            var user = await _context.Users.FindAsync(UserId);
            //await SendEmail(user);
            return View(user);
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
        public async Task<IActionResult> Create(IFormFile fileAvatar, [Bind("Name,SurName,PatronymicName,ProfilePicture,Role,Email,Password,ConfirmPassword")] RegisterModel userAccountViewModel)
        {
            //if (ModelState.IsValid)
            //{
            //    var userIsExist = await _context.Users.AnyAsync(a => a.Email == userAccountViewModel.Email);
            //    if (userIsExist == false)
            //    {
            //        try
            //        {
            //            userAccountViewModel.IsConfirmEmail = false;
            //            var pathToImage = await UploadFile(fileAvatar);

            //            userAccountViewModel.ProfilePicture = pathToImage;
            //            _context.Add(userAccountViewModel);
            //            await _context.SaveChangesAsync();

            //            await SendEmail(userAccountViewModel);
            //        }
            //        catch (Exception ex)
            //        {
            //            ModelState.AddModelError("", $"Помилка на сервері - {ex.Message}");
            //            return View(userAccountViewModel);
            //        }
            //        return Content("Для завершення реєстрації перевірте електронну пошту та перейдіть за посиланням, вказаним у листі.");

            //        //await Authenticate(userAccountViewModel.Email, userAccountViewModel.Role);

            //        //return RedirectToAction(nameof(ScheduleController.Index), "Schedule");
            //    }
            //    else
            //    {
            //        ModelState.AddModelError("", "Акаунт з цією поштою вже існує");
            //    }
            //}
            if (ModelState.IsValid) 
            {
                string? pathToImage = string.Empty;

                try
                {
                    pathToImage = await UploadFile(fileAvatar);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Помилка на сервері - {ex.Message}");
                    return View(userAccountViewModel);
                }

                ApplicationUser user = new ApplicationUser()
                {
                    Name = userAccountViewModel.Name!,
                    SurName= userAccountViewModel.SurName!,
                    PatronymicName = userAccountViewModel.PatronymicName!,
                    ProfilePicture = pathToImage,
                    Email = userAccountViewModel.Email!,
                    //Role = userAccountViewModel.Role,
                };

                var result = await userManager.CreateAsync(user, userAccountViewModel.Password!);

                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(nameof(ScheduleController.Index), "Schedule");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View(userAccountViewModel);
        }

        public async Task SendEmail(RegisterModel userAccountViewModel)
        {
            //var code = Guid.NewGuid();
            //userAccountViewModel.CodeToConfirmEmail = code;
            //_context.Update(userAccountViewModel);
            //await _context.SaveChangesAsync();
            //// генерация токена для пользователя
            //var callbackUrl = Url.Action(
            //    "ConfirmEmail",
            //    "User",
            //    new { userId = userAccountViewModel.Id, code },
            //    protocol: HttpContext.Request.Scheme);
            ////EmailService emailService = new EmailService();
            //await emailService.SendEmailAsync(userAccountViewModel.Email, $"{userAccountViewModel.Name} {userAccountViewModel.SurName}", "Confirm your account",
            //    $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>link</a>");
        }

        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            //if (userId == null || code == null)
            //{
            //    return View("Error");
            //}
            //var user = await _context.Users.FindAsync(int.Parse(userId));
            //if (user == null)
            //{
            //    return View("Error");
            //}
            ////var result = await _userManager.ConfirmEmailAsync(user, code);
            //if (Guid.Parse(code) == user.CodeToConfirmEmail)
            //{
            //    user.IsConfirmEmail = true;
            //    _context.Update(user);
            //    await _context.SaveChangesAsync();

            //    await Authenticate(user.Email, user.Role);
            //    return RedirectToAction("Index", "Schedule");
            //}
            //else
            //{
            //    return View("Error");
            //}
            return View();
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || userManager.Users == null)
            {
                return BadRequest();
            }

            var accountUser = await userManager.FindByIdAsync(id.ToString());
            //var accountUser = await userManager.Users.Where(u=>u.Id == id)
            //    .FirstOrDefaultAsync();

            if (accountUser == null)
            {
                return NotFound();
            }
            return View(accountUser);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IFormFile fileAvatar, int id, [Bind("Id,Name,SurName,PatronymicName,ProfilePicture,Role,Email,Password,CodeToConfirmEmail")] RegisterModel userAccountViewModel)
        {
            if (id != userAccountViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //var profilePicture = await _context.Users.FindAsync(id);

                    var pathToImage = await UploadFile(fileAvatar);
                    if (pathToImage != null && userAccountViewModel.ProfilePicture != null)
                    {
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", userAccountViewModel.ProfilePicture);
                        System.IO.File.Delete(path);
                    }

                    userAccountViewModel.ProfilePicture = pathToImage;

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
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Помилка на сервері - {ex.Message}");
                    return View(userAccountViewModel);
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
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(ScheduleController.Index), "Schedule");
        }

        private bool UserAccountViewModelExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        //private async Task Authenticate(string userName, Roles roles)
        //{
        //    // создаем один claim
        //    var claims = new List<Claim>
        //    {
        //        new Claim(ClaimsIdentity.DefaultNameClaimType, userName),
        //        new Claim(ClaimsIdentity.DefaultRoleClaimType, roles.ToString())
        //    };
        //    // создаем объект ClaimsIdentity
        //    ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        //    // установка аутентификационных куки
        //    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        //}

        public async Task<IActionResult> Logout()
        {
            //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(ScheduleController.Index), "Schedule");
        }

        public async Task<string> UploadFile(IFormFile fileAvatar)
        {
            //ImageFiles.Clear(); 
            // e.GetMultipleFiles(maxAllowedFiles) for several files
            int maxFileSize = 1024 * 1024 * 10; // 10MB
            if (fileAvatar.Length >= maxFileSize)
            {
                throw new Exception("Файл занадто великого розміру, максимум 10 mb");
            }
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "AvatarsImages");

            string newFileName = Path.ChangeExtension(Path.GetRandomFileName(),
                                                      Path.GetExtension(fileAvatar.FileName));

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string absolutePath = Path.Combine(path, newFileName);
            string relativePath = Path.Combine("AvatarsImages", newFileName);

            await using FileStream fs = new(absolutePath, FileMode.Create);
            await fileAvatar.OpenReadStream().CopyToAsync(fs, maxFileSize);

            return relativePath;
        }
    }
}
