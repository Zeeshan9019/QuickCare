// ==============================
// Controllers/AccountController.cs
// ==============================

using Common_QuickCare.Controllers;
using Common_QuickCare.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using QuickCare.Helpers;
using QuickCare.Repositories;
using QuickCare.ViewModel;
using System.Security.Claims;

namespace QuickCare.Controllers
{
    public class AccountController : BaseController
    {
        private readonly AccountRepository _repo;

        public AccountController(CommonRepository commonRepo, AccountRepository repo ) : base(commonRepo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserVM model)
        {
            var user = _repo.GetUserByUserName(model.UserName);

            if (user == null)
            {
                ViewBag.Error = "Invalid Username";
                return View();
            }

            bool checkPassword =
                PasswordHelper.Verify(model.Password, user.PasswordHash);

            if (!checkPassword)
            {
                ViewBag.Error = "Invalid Password";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("UserId", user.UserId.ToString())
            };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }
    }
}
