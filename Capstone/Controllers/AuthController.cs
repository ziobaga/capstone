using Capstone.Models.Auth;
using Capstone.Models.ViewModels;
using Capstone.Services.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Capstone.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authSvc;

        public AuthController(IAuthService authService)
        {
            _authSvc = authService;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel u)
        {
            try
            {
                // Chiamata al servizio per registrare l'utente
                await _authSvc.RegisterAsync(u);
                return RedirectToAction("Login", "Auth");
            }
            catch (Exception ex)
            {
                // Gestione degli errori
                ModelState.AddModelError("RegisterError", ex.Message);
                return View(u);
            }
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Mappa LoginViewModel all'entitÃ  Users
                var user = new Users
                {
                    Username = model.Email,
                    PasswordHash = model.Password
                };

                var existingUser = await _authSvc.LoginAsync(model);
                if (existingUser == null)
                {
                    ModelState.AddModelError("LoginError", "Credenziali non valide.");
                    return View(model);  // Assicurati di restituire LoginViewModel
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, existingUser.Username),
                    new Claim(ClaimTypes.NameIdentifier, existingUser.Id.ToString())
                };

                existingUser.Role.ForEach(r =>
                {
                    claims.Add(new Claim(ClaimTypes.Role, r.NomeRuolo));
                });

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Index", "Home");
            }

            return View(model);  // Assicurati di restituire LoginViewModel
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            // Esegui il logout tramite cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }

}