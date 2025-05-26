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
            {
                return BadRequest(new
                {
                    Error = "Validation failed",
                    Details = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                });
            }
            try
            {
                var result = await _empAuthService.RegisterAsync(dto);
                if (result.Succeeded)
                    return Ok(new { message = "Registration Completed successfully.", Status = "Success" });
                return BadRequest(new
                {
                    Error = "Employee registration failed",
                    Details = result.Errors.Select(e => e.Description)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = "An error occurred while registering employee",
                    Details = ex.Message
                });
            }
            
           
        }
    }
}
