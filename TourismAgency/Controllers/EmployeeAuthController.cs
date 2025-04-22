using Microsoft.AspNetCore.Mvc;
using BusinessLogic.IServices;
using DTO.Employee;
using DTO.User;


namespace TourismAgency.Controllers
{
    public class EmployeeAuthController : Controller
    {
        private readonly IEmployeeAuthService _authService;
        public EmployeeAuthController(IEmployeeAuthService authService)
        {
            _authService = authService;
        }

        // GET: /EmployeeAuth/Login
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDTO dto, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid) return View(dto);

            var result = await _authService.LoginAsync(dto);
            if (result.Succeeded)
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return RedirectToAction("Login");
        }

    }
}
