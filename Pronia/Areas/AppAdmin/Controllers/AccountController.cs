using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pronia.Utilities.Enums;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pronia.Areas.AppAdmin.Controllers
{
    [Area("AppAdmin")]
    [AutoValidateAntiforgeryToken]
    public class AccountController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IWebHostEnvironment _env;
        public readonly string fileaddress = @"admin/images/user-images";
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IWebHostEnvironment env, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _env = env;
        }
        public IActionResult Registr()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Registr(RegistrVM newuser, string ReturnUrl)
        {
            if (!ModelState.IsValid) return View();
            if (newuser.Name.Capitalize() == null)
            {
                ModelState.AddModelError("Name","Duzgun deyer daxil edin");
                return View();
            }
            if (newuser.Surname.Capitalize() == null)
            {
                ModelState.AddModelError("Surname", "Duzgun deyer daxil edin");
                return View();
            }
            if (newuser.Username.Capitalize() == null)
            {
                ModelState.AddModelError("Username", "Duzgun deyer daxil edin");
                return View();
            }
            if (!newuser.Email.IsEmail())
            {
                ModelState.AddModelError("Email", "Duzgun email daxil edin!");
                return View();
            }
            AppUser user = new AppUser
            {
                FullName = newuser.Name.Capitalize() + " "+ newuser.Surname.Capitalize(),
                UserName = newuser.Username.Capitalize(),
                Email=newuser.Email
            };

            if (newuser.Age < 0) return View();
            user.Age = newuser.Age;
            if (newuser.Gender == "Male" || newuser.Gender == "Female" || newuser.Gender == "Other")
            {
                user.Gender = newuser.Gender;
            }
            if (newuser.UserPhoto != null)
            {
                if (!newuser.UserPhoto.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "Gonderilen file-nin tipi uygun deyil");
                    return View();
                }
                if (!newuser.UserPhoto.CheckFileSize(20000))
                {
                    ModelState.AddModelError("Photo", "Gonderilen file-nin hecmi 20000 kb-den boyuk olmamalidir");
                    return View();
                }
                user.UserImgUrl = await newuser.UserPhoto.CreateFileAsync(_env.WebRootPath, fileaddress);
            }
            var result = await _userManager.CreateAsync(user, newuser.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
                return View();
            }
            await _userManager.AddToRoleAsync(user, UserRole.Admin.ToString());
            await _signInManager.SignInAsync(user, false);
            if (ReturnUrl is null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                return Redirect(ReturnUrl);
            }
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM user, string ReturnUrl)
        {
            if (!ModelState.IsValid) return View();
            AppUser existed = await _userManager.FindByEmailAsync(user.UsernameOrEmail);
            if (existed == null)
            {
                existed = await _userManager.FindByNameAsync(user.UsernameOrEmail);
                if (existed == null)
                {
                    ModelState.AddModelError(string.Empty, "Email, Username or Password is in correct!!!");
                    return View();
                }
            }
            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(existed, user.Password, user.IsRemember, true);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Email, Username or Password is in correct!!!");
                return View();
            }
            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "You are Blocked");
            }
            if (ReturnUrl is null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                return Redirect(ReturnUrl);
            }
        }
        public async Task<IActionResult> Logout(string ReturnUrl)
        {
            await _signInManager.SignOutAsync();
            if (ReturnUrl is null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                return Redirect(ReturnUrl);
            }
        }
        public async Task<IActionResult> CreateRoles()
        {
            foreach (var role in Enum.GetValues(typeof(UserRole)))
            {
                if (!(await _roleManager.RoleExistsAsync(role.ToString())))
                {
                    await _roleManager.CreateAsync(new IdentityRole
                    { Name = role.ToString() });
                }
            }
            return RedirectToAction(nameof(Index), "Home");
        }

    }
}

