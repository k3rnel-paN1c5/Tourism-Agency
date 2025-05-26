using Application.DTOs.Region;
using Application.DTOs.Trip;
using Application.DTOs.TripPlan;
using Application.DTOs.TripPlanCar;
using Application.IServices.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TourismAgency.Areas.TripSupervisor.Controllers
{
    [Area("TripSupervisor")]
    [ApiController]
    [Authorize(Roles = "TripSupervisor,Admin")]
    [Route("api/[area]/[controller]")]
    public class TripSupervisorDashboardController : ControllerBase
    {
        private readonly IRegionService _regionServ;
        private readonly ITripService _tripServ;
        private readonly ITripPlanService _tripPlanServ;
        private readonly ITripPlanCarService _carServ;
        private readonly ITripBookingService _tripBookingServ;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TripSupervisorDashboardController(
            IRegionService regionService,
            ITripService tripService,
            ITripPlanService tripPlanServ,
            ITripPlanCarService carServ,
            ITripBookingService tripBookingServ,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _regionServ = regionService;
            _tripServ = tripService;
            _tripPlanServ = tripPlanServ;
            _carServ = carServ;
            _tripBookingServ = tripBookingServ;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet] // No route parameter
        public IActionResult GetDefault()
        {
            return Ok(new { Message = "Welcome to the Trip Supervisor Dashboard" });
        }
        //* Trip Bookings *//
        [HttpPut("AcceptTripBooking/{id}")]
        public async Task<IActionResult> ConfirmTripBooking(int id){
            await _tripBookingServ.ConfirmTripBookingAsync(id);
            return Ok();
        }
        //* Regions *//

        [HttpGet("Regions")]
        public async Task<IActionResult> GetRegions(){
            var result = await _regionServ.GetAllRegionsAsync();
            return Ok(result);
        }
        [HttpGet("Regions/{id}")]
        public async Task<IActionResult> GetRegionById(int id)
        {
            try
            {
                var region = await _regionServ.GetRegionByIdAsync(id);
                if (region == null) return NotFound();
                return Ok(region);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpPost("Region")]
        public async Task<IActionResult> CreateRegion([FromBody] CreateRegionDTO dto){
            if (!ModelState.IsValid)
                return BadRequest(ModelState); 
            var newRegion = await _regionServ.CreateRegionAsync(dto);
            return CreatedAtAction(nameof(GetRegionById), new { id = newRegion.Id }, newRegion);
        }

        //* Trips *//

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
        [HttpPut("Trip/{id}")]
        public async Task<IActionResult> UpdateTrip(string id, [FromBody] UpdateTripDTO dto){
            if (!ModelState.IsValid)
                return BadRequest(ModelState); 
            try{
                await _tripServ.UpdateTripAsync(dto);
                return Ok(dto);
            }
            catch(Exception ex){
                return BadRequest(ex);
            }
        }

        //* Trip Plan *//

        [HttpGet("TripPlans")]
        public async Task<IActionResult> GetTripPlans(){
            try{
                var result = await _tripPlanServ.GetAllTripPlansAsync();
                return Ok(result);
            }
            catch(Exception ex){
                return BadRequest(ex);
            }
        }
        [HttpGet("TripPlan/{id}")]
        public async Task<IActionResult> GetTripPlanById(int id)
        {   
            try{
                var tripPlan = await _tripPlanServ.GetTripPlanByIdAsync(id);
                if (tripPlan == null) return NotFound();
                return Ok(tripPlan);
            }
            catch(Exception ex){
                return BadRequest(ex);
            }
        }
        [HttpPost("TripPlan")]
        public async Task<IActionResult> CreateTripPlan([FromBody] CreateTripPlanDTO dto){
            if (!ModelState.IsValid)
                return BadRequest(ModelState); 
            try{
                var newTripPlan = await _tripPlanServ.CreateTripPlanAsync(dto);
                return CreatedAtAction(nameof(GetTripPlanById), new { id = newTripPlan.Id }, newTripPlan);
            }
            catch(Exception ex){
                return BadRequest(ex);
            }
        }
        [HttpPut("TripPlan/{id}")]
        public async Task<IActionResult> UpdateTripPlan(string id, [FromBody] UpdateTripPlanDTO dto){
            if (!ModelState.IsValid)
                return BadRequest(ModelState); 
            try{
                await _tripPlanServ.UpdateTripPlanAsync(dto);
                return Ok(dto);
            }
            catch(Exception ex){
                return BadRequest(ex);
            }
        }

        //* Trip Plan Car *//

        [HttpGet("TripPlanCars")]
        public async Task<IActionResult> GetTripPlanCars(){
            try{
                var result = await _carServ.GetAllTripPlanCarsAsync();
                return Ok(result);
            }
            catch(Exception ex){
                return BadRequest(ex);
            }
        }
        [HttpGet("TripPlanCar/{id}")]
        public async Task<IActionResult> GetTripPlanCarById(int id)
        {   
            try{
                var tripPlanCar = await _carServ.GetTripPlanCarByIdAsync(id);
                if (tripPlanCar == null) return NotFound();
                return Ok(tripPlanCar);
            }
            catch(Exception ex){
                return BadRequest(ex);
            }
        }
        [HttpPost("TripPlanCar")]
        public async Task<IActionResult> CreateTripPlanCar([FromBody] CreateTripPlanCarDTO dto){
            if (!ModelState.IsValid)
                return BadRequest(ModelState); 
            try{
                var newTripPlanCar = await _carServ.CreateTripPlanCarAsync(dto);
                return CreatedAtAction(nameof(GetTripPlanCarById), new { id = newTripPlanCar.Id }, newTripPlanCar);
            }
            catch(Exception ex){
                return BadRequest(ex);
            }
        }
        [HttpPut("TripPlanCar/{id}")]
        public async Task<IActionResult> UpdateTripPlanCar(string id, [FromBody] UpdateTripPlanCarDTO dto){
            if (!ModelState.IsValid)
                return BadRequest(ModelState); 
            try{
                await _carServ.UpdateTripPlanCarAsync(dto);
                return Ok(dto);
            }
            catch(Exception ex){
                return BadRequest(ex);
            }
        }



    }
}
