using Application.DTOs.Region;
using Application.DTOs.Trip;
using Application.DTOs.TripPlan;
using Application.DTOs.TripPlanCar;
using Application.IServices.UseCases;
using Domain.Entities;
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

            try 
            {
                var result = await _regionServ.GetAllRegionsAsync();
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, new
                {
                    Error = "An error occurred while retrieving regions",
                    Details = e.Message
                });
            }
            
        }
        
        [HttpGet("Regions/{id}")]
        public async Task<IActionResult> GetRegionById(int id)
        {
            try
            {
                var region = await _regionServ.GetRegionByIdAsync(id);
                if (region == null) 
                    return NotFound(new {Error=$"Region with ID {id} not found" });
                return Ok(region);
            }
            catch (Exception e)
            {
                return StatusCode(500, new
                {
                    Error = $"An error occurred while retrieving region with ID {id}",
                    Details = e.Message
                });
            }
        }
       
        [HttpPost("Region")]
        public async Task<IActionResult> CreateRegion([FromBody] CreateRegionDTO dto){
            if (!ModelState.IsValid)
            {

                return BadRequest(new {
                    Error = "Validation Failed",
                    Details = ModelState.Values
                    .SelectMany(v=>v.Errors)
                    .Select(e=>e.ErrorMessage)
                    });
            }
            try
            {
                var newRegion = await _regionServ.CreateRegionAsync(dto);
                return CreatedAtAction(nameof(GetRegionById), new { id = newRegion.Id }, newRegion);

            }
            catch (Exception e)
            {
                return StatusCode(500, new
                {
                    Error = "An Error occured while creting the region",
                    Details = e.Message
                });
            }
            
        }

        //* Trips *//

        [HttpGet("Trips")]
        public async Task<IActionResult> GetTrip(){

            try
            {
                var result = await _tripServ.GetAllTripsAsync();
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, new
                {
                    Error = "An error occurred while retrieving trips",
                    Details = e.Message
                });
            }
        }

        [HttpGet("Trip/{id}")]
        public async Task<IActionResult> GetTripById(int id)
        {
            try
            {
                var trip = await _tripServ.GetTripByIdAsync(id);
                if (trip == null) return NotFound(new { Error = $"Trip with ID {id} not found" });
                return Ok(trip);
            }
            catch (Exception e)
            {
                return StatusCode(500, new
                {
                    Error = $"An error occurred while retrieving trip with ID {id}",
                    Details = e.Message
                });
            }
            
        }
        
        [HttpPost("Trip")]
        public async Task<IActionResult> CreateTrip([FromBody] CreateTripDTO dto){
            if (!ModelState.IsValid)
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
                var newTrip = await _tripServ.CreateTripAsync(dto);
                return CreatedAtAction(nameof(GetTripById), new { id = newTrip.Id }, newTrip);
            }
            catch (Exception e)
            {
                return StatusCode(500, new
                {
                    Error = "An error occurred while creating the trip",
                    Details = e.Message
                });
            }
            
        }
        
        [HttpPut("Trip/{id}")]
        public async Task<IActionResult> UpdateTrip(int id, [FromBody] UpdateTripDTO dto){
            if (!ModelState.IsValid)
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
                await _tripServ.UpdateTripAsync(dto);
                return Ok(dto);
            }
            catch( KeyNotFoundException e)
            {
                return NotFound(new { Error = e.Message });

            }
            catch(Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = $"An error occurred while updating trip with ID {id}",
                    Details = ex.Message
                });
            }
        }

        //* Trip Plan *//

        [HttpGet("TripPlans")]
        public async Task<IActionResult> GetTripPlans(){
            try{
                var result = await _tripPlanServ.GetAllTripPlansAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = "An error occurred while retrieving trip plans",
                    Details = ex.Message
                });
            }
        }
        
        [HttpGet("TripPlan/{id}")]
        public async Task<IActionResult> GetTripPlanById(int id)
        {   
            try{
                var tripPlan = await _tripPlanServ.GetTripPlanByIdAsync(id);
                if (tripPlan == null) return NotFound(new { Error = $"Trip Plan with ID {id} not found" });
                return Ok(tripPlan);
            }
            catch(Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = $"An error occurred while retrieving trip plan with ID {id}",
                    Details = ex.Message
                });
            }
        }
        
        [HttpPost("TripPlan")]
        public async Task<IActionResult> CreateTripPlan([FromBody] CreateTripPlanDTO dto){
            if (!ModelState.IsValid)
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
                var newTripPlan = await _tripPlanServ.CreateTripPlanAsync(dto);
                return CreatedAtAction(nameof(GetTripPlanById), new { id = newTripPlan.Id }, newTripPlan);
            }
            catch(Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = "An error occurred while creating the trip plan",
                    Details = ex.Message
                });
            }
        }
        
        [HttpPut("TripPlan/{id}")]
        public async Task<IActionResult> UpdateTripPlan(int id, [FromBody] UpdateTripPlanDTO dto){
            if (!ModelState.IsValid)
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
                await _tripPlanServ.UpdateTripPlanAsync(dto);
                return Ok(dto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = $"An error occurred while updating trip plan with ID {id}",
                    Details = ex.Message
                });
            }
        }

        //* Trip Plan Car *//

        [HttpGet("TripPlanCars")]
        public async Task<IActionResult> GetTripPlanCars(){
            try{
                var result = await _carServ.GetAllTripPlanCarsAsync();
                return Ok(result);
            }
            catch(Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = "An error occurred while retrieving trip plan cars",
                    Details = ex.Message
                });
            }
        }
        [HttpGet("TripPlanCar/{id}")]
        public async Task<IActionResult> GetTripPlanCarById(int id)
        {   
            try{
                var tripPlanCar = await _carServ.GetTripPlanCarByIdAsync(id);
                if (tripPlanCar == null) return NotFound(new { Error = $"Trip Plan Car with ID {id} not found" });
                return Ok(tripPlanCar);
            }
            catch(Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = $"An error occurred while retrieving trip plan car with ID {id}",
                    Details = ex.Message
                });
            }
        }
        
        [HttpPost("TripPlanCar")]
        public async Task<IActionResult> CreateTripPlanCar([FromBody] CreateTripPlanCarDTO dto){
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Error = "Validation failed",
                    Details = ModelState.Values
                       .SelectMany(v => v.Errors)
                       .Select(e => e.ErrorMessage)
                });
            }
            try{
                var newTripPlanCar = await _carServ.CreateTripPlanCarAsync(dto);
                return CreatedAtAction(nameof(GetTripPlanCarById), new { id = newTripPlanCar.Id }, newTripPlanCar);
            }
            catch(Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = "An error occurred while creating the trip plan car",
                    Details = ex.Message
                });
            }
        }

        [HttpPut("TripPlanCar/{id}")]
        public async Task<IActionResult> UpdateTripPlanCar(int id, [FromBody] UpdateTripPlanCarDTO dto){
            if (!ModelState.IsValid)
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
                await _carServ.UpdateTripPlanCarAsync(dto);
                return Ok(dto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = $"An error occurred while updating trip plan car with ID {id}",
                    Details = ex.Message
                });
            }
        }



    }
}
