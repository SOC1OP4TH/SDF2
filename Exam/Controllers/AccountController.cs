using Exam.DAL;
using Exam.Models;
using Exam.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

namespace Exam.Controllers
{
    public class AccountController : Controller
    {
        UserManager<User> _userManager;
        SignInManager<User> _signInManager;
        AppDbContext _db;
        
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, AppDbContext appDbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = appDbContext;
        }

        public IActionResult Index()
        {
            return View(_db.Users);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Incorrect UserName or Password");
                return View(loginVM);
            }
            var res = await _userManager.FindByNameAsync(loginVM.UserName);
            if (res == null)
            {
                ModelState.AddModelError("", "Incorrect UserName or Password");
                return View(loginVM);
            }
            var SignInRes = await _signInManager.PasswordSignInAsync(res, loginVM.Password, loginVM.RememberMe,true);
            if (!SignInRes.Succeeded)
            {
                ModelState.AddModelError("", "Incorrect UserName or Password");
                return View(loginVM);
            }

            return RedirectToAction("Index", "Account");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }
            User ExsistUser = await _userManager.FindByNameAsync(registerVM.UserName);

            if (ExsistUser != null)
            {
                ModelState.AddModelError("UserName", "This UserName is exsist");
                return View();
            }

            User user = new User()
            {
                UserName = registerVM.UserName,
                Name = registerVM.Name,
                Surname = registerVM.Surname,
                Email = registerVM.Email,
            };

            var res = await _userManager.CreateAsync(user, registerVM.Password);
            if (!res.Succeeded)
            {
                foreach (var item in res.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                
                return View();
            }

            await _signInManager.SignInAsync(user, true);
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Index), "Home");
        }
    }
}
