using Application.DTOs.Region;
using Application.DTOs.Trip;
using Application.DTOs.TripPlan;
using Application.DTOs.TripPlanCar;
using Application.IServices.UseCases;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TourismAgency.Areas.TripSupervisor.Controllers
{
    [Area("TripSupervisor")]
    [ApiController]
    [Authorize(Roles = "TripSupervisor,Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[area]/[controller]")]
    public class TripSupervisorDashboardController : ControllerBase
    {
        private readonly IRegionService _regionServ;
        private readonly ITripService _tripServ;
        private readonly ITripPlanService _tripPlanServ;
        private readonly ITripPlanCarService _tripPlanCarService;
        private readonly ITripBookingService _tripBookingServ;
        private readonly ICarService _carService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TripSupervisorDashboardController(
            IRegionService regionService,
            ITripService tripService,
            ITripPlanService tripPlanServ,
            ITripPlanCarService carServ,
            ITripBookingService tripBookingServ,
            ICarService carService,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _regionServ = regionService;
            _tripServ = tripService;
            _tripPlanServ = tripPlanServ;
            _tripPlanCarService = carServ;
            _carService = carService;
            _tripBookingServ = tripBookingServ;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet] // No route parameter
        public IActionResult GetDefault()
        {
            return Ok(new { Message = "Welcome to the Trip Supervisor Dashboard" });
        }
        //* Trip Bookings *//
        [HttpPut("Accept/{id}")]
        public async Task<IActionResult> Confirm(int id){
            await _tripBookingServ.ConfirmTripBookingAsync(id);
            return Ok();
        }
        [HttpPut("Cancel/{id}")]
        public async Task<IActionResult> Cancel(int id){
            await _tripBookingServ.CancelTripBookingAsync(id);
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
       
        [HttpPost("Regions")]
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
        [HttpPut("Regions/{id}")]
        public async Task<IActionResult> UpdateRegion(int id, [FromBody] UpdateRegionDTO dto){
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
                await _regionServ.UpdateRegionAsync(dto);
                return Ok(dto);
            }
            catch( KeyNotFoundException e)
            {
                return NotFound(new { Error = e.Message });

            }
            catch( DbUpdateException e)
            {
                return Conflict(new { Error = e.Message });

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
        [HttpDelete("Regions/{id}")]
        public async Task<IActionResult> DeleteRegion(int id)
        {
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
                await _regionServ.DeleteRegionAsync(id);
                return Ok($"Deleted A trip with id {id}");
            }
            catch( KeyNotFoundException e)
            {
                return NotFound(new { Error = e.Message });

            }
            catch( DbUpdateException e)
            {
                return Conflict(new { Error = e.Message });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = $"An error occurred while Deleting Region with ID {id}",
                    Details = ex.Message
                });
            }
        }
        //* Trips *//

        [HttpGet("Trips")]
        public async Task<IActionResult> GetTrips(){

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

        [HttpGet("Trips/{id}")]
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
        [HttpGet("AvailableTrips")]
        public async Task<IActionResult> GetAvailableTrips(){

            try
            {
                var result = await _tripServ.GetAvailablePublicTripsAsync();
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, new
                {
                    Error = "An error occurred while retrieving Available and public trips",
                    Details = e.Message
                });
            }
        }
        
        [HttpPost("Trips")]
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
        
        [HttpPut("Trips/{id}")]
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

        [HttpDelete("Trips/{id}")]
        public async Task<IActionResult> DeleteTrip(int id)
        {
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
                await _tripServ.DeleteTripAsync(id);
                return Ok($"Deleted A trip with id {id}");
            }
            catch( KeyNotFoundException e)
            {
                return NotFound(new { Error = e.Message });

            }
            catch( DbUpdateException e)
            {
                return Conflict(new { Error = e.Message });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = $"An error occurred while Deleting trip with ID {id}",
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
        
        [HttpGet("TripPlans/{id}")]
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
        
        [HttpPost("TripPlans")]
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
        
        [HttpPut("TripPlans/{id}")]
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

        [HttpDelete("TripPlans/{id}")]
        public async Task<IActionResult> DeleteTripPlan(int id)
        {
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
                await _tripPlanServ.DeleteTripPlanAsync(id);
                return Ok($"Deleted A trip Plan with id {id}");
            }
            catch( KeyNotFoundException e)
            {
                return NotFound(new { Error = e.Message });

            }
            catch( DbUpdateException e)
            {
                return Conflict(new { Error = e.Message });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = $"An error occurred while Deleting trip with ID {id}",
                    Details = ex.Message
                });
            }
        }

        //* Trip Plan Car *//


        [HttpGet("AvailableCars")]
        public async Task<IActionResult> GetAvailableCarsAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var result = await _carService.GetAvailableCarsAsync(startDate, endDate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = "Failed to retrieve available cars",
                    Details = ex.Message
                });
            }
        }
        [HttpGet("Cars")]
        
        public async Task<IActionResult> GetCars()
        {
            try
            {
                var result = await _carService.GetAllCarsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = "An error occurred while retrieving cars",
                    Details = ex.Message
                });
            }

        }
        
        [HttpPost("AddTripPlanCar")]
        public async Task<IActionResult> AddTripPlanCar([FromBody] CreateTripPlanCarFromTripPlanDTO dto){
            try{
                var result = await _tripPlanServ.AddCarToTripPlanAsync(dto);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = "An error occurred while adding trip plan car to a trip plan",
                    Details = ex.Message
                });
            }
        }
        [HttpPost("RemoveTripPlanCar/{id}")]
        public async Task<IActionResult> RemoveTripPlanCar(int id){
            try{
                await _tripPlanServ.RemoveCarFromTripPlanAsync(id);
                return Ok();
            }
            catch(Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = "An error occurred while removing trip plan car from a trip plan",
                    Details = ex.Message
                });
            }
        }

        [HttpGet("TripPlanCars")]
        public async Task<IActionResult> GetTripPlanCars(){
            try{
                var result = await _tripPlanCarService.GetAllTripPlanCarsAsync();
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
        [HttpGet("TripPlanCars/{id}")]
        public async Task<IActionResult> GetTripPlanCarById(int id)
        {   
            try{
                var tripPlanCar = await _tripPlanCarService.GetTripPlanCarByIdAsync(id);
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
        
        [HttpPost("TripPlanCars")]
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
                var newTripPlanCar = await _tripPlanCarService.CreateTripPlanCarAsync(dto);
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

        [HttpPut("TripPlanCars/{id}")]
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
                await _tripPlanCarService.UpdateTripPlanCarAsync(dto);
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

        [HttpDelete("TripPlanCars/{id}")]
        public async Task<IActionResult> DeleteTripPlanCar(int id)
        {
            try
            {
                await _tripPlanCarService.DeleteTripPlanCarAsync(id);
                return Ok($"Deleted A trip plan car with id {id}");
            }
            catch( KeyNotFoundException e)
            {
                return NotFound(new { Error = e.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = $"An error occurred while Deleting trip plan car with ID {id}",
                    Details = ex.Message
                });
            }
        }
    }
}
