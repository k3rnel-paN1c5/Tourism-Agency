using Application.DTOs.Region;
using Application.DTOs.Trip;
using Application.IServices.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TourismAgency.Areas.TripSupervisor.Controllers
{
    [Area("TripSupervisor")]
    [ApiController]
    // [Authorize(Roles = "TripSupervisor")]
    [Route("api/[area]/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IRegionService _regionServ;
        private readonly ITripService _tripServ;
        public DashboardController(IRegionService regionService, ITripService tripService){
            _regionServ = regionService;
            _tripServ = tripService;
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
        [HttpGet("Regions/{id}")]
        public async Task<IActionResult> GetRegionById(int id)
        {
            var region = await _regionServ.GetRegionByIdAsync(id);
            if (region == null) return NotFound();
            return Ok(region);
        }
        [HttpPost("Region")]
        public async Task<IActionResult> CreateRegion([FromBody] CreateRegionDTO dto){
            if (!ModelState.IsValid)
                return BadRequest(ModelState); 
            var newRegion = await _regionServ.CreateRegionAsync(dto);
            return CreatedAtAction(nameof(GetRegionById), new { id = newRegion.Id }, newRegion);
        }
        [HttpGet("Trips")]
        public async Task<IActionResult> GetTrip(){
            var result = await _tripServ.GetAllTripsAsync();
            return Ok(result);
        }
        [HttpGet("Trip/{id}")]
        public async Task<IActionResult> GetTripById(int id)
        {
            var trip = await _tripServ.GetTripByIdAsync(id);
            if (trip == null) return NotFound();
            return Ok(trip);
        }
        [HttpPost("Trip")]
        public async Task<IActionResult> CreateTrip([FromBody] CreateTripDTO dto){
            if (!ModelState.IsValid)
                return BadRequest(ModelState); 
            var newTrip = await _tripServ.CreateTripAsync(dto);
            return CreatedAtAction(nameof(GetTripById), new { id = newTrip.Id }, newTrip);
        }

    }
}
