using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.IServices.Auth;
using Microsoft.AspNetCore.Authorization;
using Application.DTOs.Employee;

namespace TourismAgency.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[area]/[controller]")]
    public class AdminDashboardController : ControllerBase
    {
        private readonly IEmployeeAuthService _empAuthService;
        public AdminDashboardController(IEmployeeAuthService empAuthService){
            _empAuthService  = empAuthService;
        }
        [HttpGet] // No route parameter
        public IActionResult GetDefault()
        {
            return Ok(new { Message = "Welcome to the ADMIN Dashboard" });
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterEmp([FromBody] EmployeeRegisterDTO dto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _empAuthService.RegisterAsync(dto);
            if(result.Succeeded)
                return Ok(new { message = "Registration Completed successfully." });
            return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
        }
    }
}
