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
    [Route("api/customer")]
    public class CustomerAuthController : ControllerBase
    {
        private readonly ICustomerAuthService _authService;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        public CustomerAuthController(ICustomerAuthService authService, IJwtTokenGenerator jwtTokenGenerator)
        {
            _authService = authService;
            _jwtTokenGenerator = jwtTokenGenerator;

        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(new { message = "CORS working!" });
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CustomerRegisterDTO dto)
        {
           
                
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(dto);

            if (result.Succeeded)
            {
                // Optionally log in after registration
                var loginResult = await _authService.LoginAsync(new LoginDTO
                {
                    Email = dto.Email,
                    Password = dto.Password,
                    RememberMe = false
                });
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
                var token =  _jwtTokenGenerator.GenerateToken(userId!, dto.Email!, role!);
                if (loginResult.Succeeded)
                {
                    return Ok(new
                    {
                        status = 200,
                        result = loginResult,
                        isSuccess = true,
                        Token = token,
                        message = "Registration and login successful."
                    });
                }
            }

            return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
        }

                // POST: api/CustomerAuth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto, [FromQuery] string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
                return BadRequest(new
                {
                    error = "Already logged in. Please logout first."
                }); 

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
                    status = 200,
                    isSuccess = true,
                    Token = token,
                    message = "Login successful.",
                    result,
                    redirectUrl = !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)
                        ? returnUrl
                        : "/api/customer/customerdashboard"
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
