using Application.DTOs.TripBooking;
using Application.DTOs.CarBooking;
using Application.IServices.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace TourismAgency.Areas.Customer.Controllers
{
    [Area("Customer")]
    [ApiController]
    // [Authorize]
    [Route("api/[area]/[controller]")]
    public class CustomerDashboardController : ControllerBase
    {
        private readonly ITripBookingService _tripBookingService;
        private readonly ICarBookingService _carBookingService;

        public CustomerDashboardController(ITripBookingService tripBookingService, ICarBookingService carBookingService)
        {
            _tripBookingService = tripBookingService;
            _carBookingService = carBookingService;
        }

        [HttpGet]
        public IActionResult GetDefault()
        {
            var isAuthenticated = User.Identity?.IsAuthenticated;
            var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();

            return Ok(new
            {
                Message = "Welcome to the Customer Dashboard",
                IsAuthenticated = isAuthenticated,
                Claims = claims
            });
        }

        //* Trip Booking *//

        [HttpGet("TripBooking")]
        public async Task<IActionResult> GetTripBookings()
        {
            try
            {
                var result = await _tripBookingService.GetAllTripBookingsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet("TripBooking/{id}")]
        public async Task<IActionResult> GetTripBookingById(int id)
        {
            try
            {
                var tripBooking = await _tripBookingService.GetTripBookingByIdAsync(id);
                if (tripBooking == null) return NotFound("trip booking not found");
                return Ok(tripBooking);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("TripBooking")]
        public async Task<IActionResult> CreateTripBooking([FromBody] CreateTripBookingDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var newTripBooking = await _tripBookingService.CreateTripBookingAsync(dto);
                return CreatedAtAction(nameof(GetTripBookingById), new { id = newTripBooking.Id }, newTripBooking);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("TripBooking/{id}")]
        public async Task<IActionResult> UpdateTripBooking(string id, [FromBody] UpdateTripBookingDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                await _tripBookingService.UpdateTripBookingAsync(dto);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //* Car Booking *//

        [HttpGet("CarBooking")]
        public async Task<IActionResult> GetCarBookings()
        {
            try
            {
                var result = await _carBookingService.GetAllCarBookingsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("CarBooking/{id}")]
        public async Task<IActionResult> GetCarBookingById(int id)
        {
            try
            {
                var carBooking = await _carBookingService.GetCarBookingByIdAsync(id);
                if (carBooking == null) return NotFound("car booking not found");
                return Ok(carBooking);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("CarBooking")]
        public async Task<IActionResult> CreateCarBooking([FromBody] CreateCarBookingDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var newCarBooking = await _carBookingService.CreateCarBookingAsync(dto);
                return CreatedAtAction(nameof(GetCarBookingById), new { id = newCarBooking.Id }, newCarBooking);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("CarBooking/{id}")]
        public async Task<IActionResult> UpdateCarBooking(string id, [FromBody] UpdateCarBookingDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                await _carBookingService.UpdateCarBookingAsync(dto);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
