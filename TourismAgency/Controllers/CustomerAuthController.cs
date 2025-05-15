using Microsoft.AspNetCore.Mvc;
using Application.IServices.Auth;
using Application.DTOs.Customer;
using Application.DTOs.User;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Authentication;
using System.Security.Claims;
namespace TourismAgency.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerAuthController : ControllerBase
    {
        private readonly ICustomerAuthService _authService;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        public CustomerAuthController(ICustomerAuthService authService, IJwtTokenGenerator jwtTokenGenerator)
        {
            _authService = authService;
            _jwtTokenGenerator = jwtTokenGenerator;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CustomerRegisterDTO dto)
        {
           
                
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(new CustomerRegisterDTO
            {
                Email = dto.Email,
                Password = dto.Password,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                Whatsapp = dto.Whatsapp,
                Country = dto.Country
            });

            if (result.Succeeded)
            {
                // Optionally log in after registration
                var loginResult = await _authService.LoginAsync(new LoginDTO
                {
                    Email = dto.Email,
                    Password = dto.Password,
                    RememberMe = false
                });

                if (loginResult.Succeeded)
                {
                    return Ok(new { message = "Registration and login successful." });
                }
            }

            return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
        }

                // POST: api/CustomerAuth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto, [FromQuery] string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
                return BadRequest(new { error = "Already logged in. Please logout first." }); 

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(dto);

            if (result.Succeeded)
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
                var token = _jwtTokenGenerator.GenerateToken(userId!, dto.Email!, role!);
                return Ok(new
                {
                    Token = token,
                    message = "Login successful.",
                    redirectUrl = !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)
                        ? returnUrl
                        : "/home"
                });
            }

            return Unauthorized(new { error = "Invalid login attempt." });
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return Ok(new { message = "Logout successful." });
        }

    }

}
