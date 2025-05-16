using Application.DTOs.TripBooking;
using Application.IServices.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TourismAgency.Areas.Customer.Controllers
{
    [Area("Customer")]
    [ApiController]
    [Route("api/[area]/[controller]")]
    public class CustomerDashboardController : ControllerBase
    {
        private readonly ITripBookingService _tripBookingService;
        public CustomerDashboardController(ITripBookingService tripBookingService)
        {
            _tripBookingService = tripBookingService;
        }

        [HttpGet] // No route parameter
        public IActionResult GetDefault()
        {
            return Ok(new { Message = "Welcome to the Normal Dashboard" });
        }

        //* Trip Booking *//

        [HttpGet("TripBooking")]
        public async Task<IActionResult> GetTripBookings(){
            try{
                var result = await _tripBookingService.GetAllTripBookingsAsync();
                return Ok(result);
            }
            catch(Exception ex){
                return BadRequest(ex);
            }
        }
        [HttpGet("TripBooking/{id}")]
        public async Task<IActionResult> GetTripBookingById(int id)
        {   
            try{
                var tripBooking = await _tripBookingService.GetTripBookingByIdAsync(id);
                if (tripBooking == null) return NotFound();
                return Ok(tripBooking);
            }
            catch(Exception ex){
                return BadRequest(ex);
            }
        }
        [HttpPost("TripBooking")]
        public async Task<IActionResult> CreateTripBooking([FromBody] CreateTripBookingDTO dto){
            if (!ModelState.IsValid)
                return BadRequest(ModelState); 
            try{
                var newTripBooking = await _tripBookingService.CreateTripBookingAsync(dto);
                return CreatedAtAction(nameof(GetTripBookingById), new { id = newTripBooking.Id }, newTripBooking);
            }
            catch(Exception ex){
                return BadRequest(ex);
            }
        }
        [HttpPut("TripBooking/{id}")]
        public async Task<IActionResult> UpdateTripBooking(string id, [FromBody] UpdateTripBookingDTO dto){
            if (!ModelState.IsValid)
                return BadRequest(ModelState); 
            try{
                await _tripBookingService.UpdateTripBookingAsync(dto);
                return Ok(dto);
            }
            catch(Exception ex){
                return BadRequest(ex);
            }
        }
    }
}
