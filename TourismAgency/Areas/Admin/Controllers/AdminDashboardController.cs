using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.IServices.Auth;
using Microsoft.AspNetCore.Authorization;

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
    }
}
