﻿using Microsoft.AspNetCore.Mvc;
using Application.IServices.Auth;
using Application.DTOs.Employee;
using Application.DTOs.User;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace TourismAgency.Controllers
{
    public class EmployeeAuthController : Controller
    {
        private readonly IEmployeeAuthService _authService;
        public EmployeeAuthController(IEmployeeAuthService authService)
        {
            _authService = authService;
        }
        // GET: /EmployeeAuth/Register
        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.Roles = new SelectList(
            Enum.GetValues(typeof(EmployeeRegisterDTO.TRoles)).Cast<EmployeeRegisterDTO.TRoles>());

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(EmployeeRegisterDTO dto)
        {
            if (!ModelState.IsValid)
            {    
                ViewBag.Roles = new SelectList(Enum.GetValues(typeof(EmployeeRegisterDTO.TRoles)).Cast<EmployeeRegisterDTO.TRoles>());
                return View(dto);
            }

            var result = await _authService.RegisterAsync(new EmployeeRegisterDTO
            {
                Email = dto.Email,
                Password = dto.Password,

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
            return View(dto);
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
