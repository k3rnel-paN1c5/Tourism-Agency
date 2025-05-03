using Application.DTOs.Region;
using Application.IServices.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TourismAgency.Areas.TripSupervisor.Controllers
{
    [Area("TripSupervisor")]
    [ApiController]
    [Authorize(Roles = "TripSupervisor")]
    [Route("api/[area]/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IRegionService _regionServ;
        public DashboardController(IRegionService regionService){
            _regionServ = regionService;
        }
        [HttpGet] // No route parameter
        public IActionResult GetDefault()
        {
            return Ok(new { Message = "Welcome to the Dashboard" });
        }
        [HttpGet("Regions")]
        public async Task<IActionResult> GetRegions(){
            var result = await _regionServ.GetAllRegionsAsync();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRegionById(int id)
        {
            var region = await _regionServ.GetRegionByIdAsync(id);
            if (region == null) return NotFound();
            return Ok(region);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRegion([FromBody] CreateRegionDTO dto){
            if (!ModelState.IsValid)
                return BadRequest(ModelState); 
            var newRegion = await _regionServ.CreateRegionAsync(dto);
            return CreatedAtAction(nameof(GetRegionById), new { id = newRegion.Id }, newRegion);
        }

    }
}
