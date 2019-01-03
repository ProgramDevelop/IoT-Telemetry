using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Telemetry.Database;
using Telemetry.Database.Models;
using Telemetry.Web.Services.Auth;
using Telemetry.Web.ViewModels.User;

namespace Telemetry.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;

        public UserController(IAuthService authService, IUserRepository repository)
        {
            _authService = authService;
            _userRepository = repository;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                if (_authService.CheckCredentials(model.Email, model.Password))
                {
                    var user = _userRepository.GetUserByEmail(model.Email);
                    await Authenticate(user); // аутентификация
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                if (_authService.Register(model.Email, model.Password))
                {
                    var user = _userRepository.GetUserByEmail(model.Email);
                    await Authenticate(user); // аутентификация
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("EmailExist", "Данный Email уже зарегистрирован");
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        private async Task Authenticate(User user)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}