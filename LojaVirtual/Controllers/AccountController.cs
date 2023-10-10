using LojaVirtual.Models.ModelsUsuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Net.Http;

namespace LojaVirtual.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            var UserProfile = _userManager.Users.ToList();
            ViewData["Id"] = new SelectList(UserProfile, "Id", "UserName");

            return View(new LoginUser()
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginUser loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }

            var userId = await _userManager.FindByIdAsync(loginVM.UserId);
            if (userId != null)
            {
                var result = await _signInManager.PasswordSignInAsync(userId, loginVM.Password, false, false);
                if (result.Succeeded)
                {
                    if (string.IsNullOrEmpty(loginVM.ReturnUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    return Redirect(loginVM.ReturnUrl);
                }
            }
            else
            {
                ModelState.AddModelError("", "Falha ao realizar o login!!");
            }

            var UserProfile = _userManager.Users.ToList();
            ViewData["Id"] = new SelectList(UserProfile, "Id", "UserName", userId);
            return View(loginVM);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.User = null;
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
