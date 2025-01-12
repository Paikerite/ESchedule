using Microsoft.AspNetCore.Mvc;
using ESchedule.Data;
using ESchedule.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ESchedule.Services;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using System.Text;

namespace ESchedule.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<LoginModel> logger;
        private readonly EmailService emailService;

        public AccountController(EScheduleDbContext context, EmailService emailService, UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager, ILogger<LoginModel> logger)
        {
            this.emailService = emailService;
            userManager = _userManager;
            signInManager = _signInManager;
            this.logger = logger;
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
            if (id == null || userManager.Users == null)
            {
                return NotFound();
            }

            var userAccountViewModel = await userManager.FindByIdAsync(id.ToString());
            if (userAccountViewModel == null)
            {
                return NotFound("Юзер заданого id не знайдений");
            }

            return View(userAccountViewModel);
        }

        // GET: User/Login
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login([Bind("Email,Password,RememberMe")] LoginModel model, string? returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await signInManager.PasswordSignInAsync(model.Email!, model.Password!, model.RememberMe, false);

                if (result.Succeeded)
                {
                    logger.LogInformation("User logged in.");
                    if (returnUrl is null)
                    {
                        return RedirectToAction(nameof(ScheduleController.Index), "Schedule");
                    }
                    else
                    {
                        return LocalRedirect(returnUrl);
                    }
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToAction("LoginWith2fa", new { ReturnUrl = returnUrl, model.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    logger.LogWarning("User account locked out.");
                    return RedirectToAction("Lockout");
                }

                ModelState.AddModelError("", "Невдала спроба ввійти");
            }
            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> MustConfirmEmail(int Id, string? returnUrl = null)
        {
            var user = await userManager.FindByIdAsync(Id.ToString());
            if (user == null)
            {
                return RedirectToAction("Login");
            }
            await SendEmail(user, returnUrl);
            return View(user);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                //return RedirectToAction(nameof(ScheduleController.Index), "Schedule");
                return View("Bad userID or code");
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Не вдалося завантажити юзера з таким ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await userManager.ConfirmEmailAsync(user, code);
            var StatusMessage = result.Succeeded ? "Дякую за підтвердження пошти!" : "Error. Сталося помилка під час підтвердження пошти";
            ViewBag.StatusMessage = StatusMessage;
            return View();
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
        public async Task<IActionResult> Create(IFormFile fileAvatar, [Bind("Name,SurName,PatronymicName,ProfilePicture,Role,Email,Password,ConfirmPassword")] RegisterModel userAccountViewModel, string? returnUrl = null)
        {
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
                    SurName = userAccountViewModel.SurName!,
                    PatronymicName = userAccountViewModel.PatronymicName!,
                    ProfilePicture = pathToImage,
                    Email = userAccountViewModel.Email!,
                    //Role = userAccountViewModel.Role,
                };

                var result = await userManager.CreateAsync(user, userAccountViewModel.Password!);

                if (result.Succeeded)
                {
                    logger.LogInformation("User created a new account with password.");

                    await SendEmail(user, returnUrl);

                    if (userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToAction("MustConfirmEmail", new { email = userAccountViewModel.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }

                    //await signInManager.SignInAsync(user, isPersistent: false);
                    //return RedirectToAction(nameof(ScheduleController.Index), "Schedule");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View(userAccountViewModel);
        }

        public async Task SendEmail(ApplicationUser user, string? returnUrl)
        {
            var userId = await userManager.GetUserIdAsync(user);
            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Account", userId = userId, code = code, returnUrl = returnUrl },
                protocol: Request.Scheme);

            await emailService.SendEmailAsync(user.Email, "Підтвердити пошту",
                $"Будь ласка, підтвердіть пошту: <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>натисніть тут</a>.");
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || userManager.Users == null)
            {
                return NotFound();
            }

            var accountUser = await userManager.FindByIdAsync(id.ToString());
            if (accountUser == null)
            {
                return NotFound("Юзер заданого id не знайдений");
            }

            EditModel editModel = new EditModel() 
            {
                Id=accountUser.Id,
                Name =accountUser.Name,
                SurName = accountUser.SurName,
                PatronymicName = accountUser.PatronymicName,
                Email = accountUser.Email,
                ProfilePicture = accountUser.ProfilePicture,
            };

            return View(editModel);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IFormFile fileAvatar, int id, [Bind("Id,Name,SurName,PatronymicName,ProfilePicture,Role,Email,,CodeToConfirmEmail")] EditModel editModel)
        {
            if (id != editModel.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(id.ToString());
                var result = await signInManager.CheckPasswordSignInAsync(user!, editModel.Password!, false);

                if (result.Succeeded)
                {
                    try
                    {
                        var pathToImage = await UploadFile(fileAvatar);
                        if (pathToImage != null && editModel.ProfilePicture != null)
                        {
                            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", editModel.ProfilePicture);
                            System.IO.File.Delete(path);
                        }
                        editModel.ProfilePicture = pathToImage;
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", $"Помилка на сервері - {ex.Message}");
                        return View(editModel);
                    }

                    user.Name = editModel.Name;
                    user.SurName = editModel.SurName;
                    user.PatronymicName = editModel.PatronymicName;
                    user.ProfilePicture = editModel.ProfilePicture;
                    user.Email = editModel.Email;

                    var resultupdate = await userManager.UpdateAsync(user);
                    await signInManager.RefreshSignInAsync(user);
                }
                return RedirectToAction(nameof(ScheduleController.Index), "Schedule");
            }
            return View(editModel);
        }

        //GET: User/NotHaveRights
        public IActionResult NotHaveRights()
        {
            return View();
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || userManager.Users == null)
            {
                return NotFound();
            }

            var userAccountViewModel = await userManager.FindByIdAsync(id.ToString());
            if (userAccountViewModel == null)
            {
                return NotFound("Юзер заданого id не знайдений");
            }

            return View(new InputPasswordModel { Id = userAccountViewModel.Id, Name = userAccountViewModel.Name});
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([Bind("Id,Name,Password")] InputPasswordModel model)
        {
            if (userManager.Users == null)
            {
                return NotFound();
            }
            var user = await userManager.FindByIdAsync(model.Id.ToString());
            if (user == null)
            {
                return NotFound("Юзер заданого id не знайдений");
            }

            if (!await userManager.CheckPasswordAsync(user, model.Password))
            {
                ModelState.AddModelError(string.Empty, "Невірно введений пароль.");
                return View();
            }

            var result = await userManager.DeleteAsync(user);
            var userId = await userManager.GetUserIdAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Трапилась помилка під час видалення юзера {userId}");
            }

            await signInManager.SignOutAsync();

            logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId);

            return RedirectToAction(nameof(ScheduleController.Index), "Schedule");
        }

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

        public IActionResult ForgotPassword()
        {
            return View();
        }

        public IActionResult ForgotPasswordConfirmation() 
        {
            return View();
        }

        [HttpPost, ActionName("ForgotPasswordPost")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword([Bind("Email")] ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToAction("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Account", code },
                    protocol: Request.Scheme);

                await emailService.SendEmailAsync(
                    model.Email,
                    "Скидання паролю",
                    $"Будь ласка, виконайте скидання <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>натисніть тут</a>.");

                return RedirectToAction("ForgotPasswordConfirmation");
            }

            return View();
        }

        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                return BadRequest("A code must be supplied for password reset.");
            }
            else
            {
                ResetPasswordModel input = new ResetPasswordModel
                {
                    Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code))
                };
                return View(input);
            }
        }

        [HttpPost, ActionName("ResetPasswordPost")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword([Bind("Email, Password, ConfirmPassword, Code")] ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation");
            }

            var result = await userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View();
        }
    }
}
