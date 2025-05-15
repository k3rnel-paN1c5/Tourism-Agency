using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TourismAgency.Areas.Customer.Controllers
{
    [Area("Customer")]
    [ApiController]
    [Route("api/[area]/[controller]")]
    public class CustomerDashboardController : ControllerBase
    {
        [HttpGet] // No route parameter
        public IActionResult GetDefault()
        {
            return Ok(new { Message = "Welcome to the Normal Dashboard" });
        }
    }
}
