using Microsoft.AspNetCore.Mvc;
using Application.IServices.Auth;
using Application.DTOs.Customer;
using Application.DTOs.User;
namespace TourismAgency.Controllers
{
    public class CustomerAuthController : Controller
    {
        private readonly ICustomerAuthService _authService;
        public CustomerAuthController(ICustomerAuthService authService)
        {
            _authService = authService;
        }
        [HttpGet]
        public  IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(CustomerRegisterDTO dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.RegisterAsync(new CustomerRegisterDTO
                {
                    Email = dto.Email,
                    Password = dto.Password,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    PhoneNumber = dto.PhoneNumber,
                    Whatsapp = dto.Whatsapp,
                    Country = dto.Country,
                    // Role = "Customer" // Default role for customers
                });

                if (result.Succeeded)
                {
                    // Automatically log in the user after registration
                    await _authService.LoginAsync(new LoginDTO
                    {
                        Email = dto.Email,
                        Password = dto.Password,
                        RememberMe = false
                    });

                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(dto);
        }
        // GET: /CustomerAuth/Login
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
