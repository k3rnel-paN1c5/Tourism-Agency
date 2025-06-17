using Application.DTOs.TripBooking;
using Application.DTOs.CarBooking;
using Application.DTOs.Payment;
using Application.IServices.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Infrastructure.Authentication;
using Application.Services.UseCases;
using Domain.Enums;

namespace TourismAgency.Areas.Customer.Controllers
{
    [Area("Customer")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[area]/[controller]")]
    public class CustomerDashboardController : ControllerBase
    {
        private readonly ITripBookingService _tripBookingService;
        private readonly ICarBookingService _carBookingService;
        private readonly ITripPlanService _tripPlanService;
        private readonly ICarService _carService;
        private readonly ICategoryService _categoryService;
        private readonly IPaymentService _paymentService;

        public CustomerDashboardController(
            ITripBookingService tripBookingService,
            ICarBookingService carBookingService,
            ITripPlanService tripPlanService,
            ICarService carService,
            ICategoryService categoryService,
            IPaymentService paymentService
            )
        {
            _tripBookingService = tripBookingService;
            _carBookingService = carBookingService;
            _tripPlanService = tripPlanService;
            _carService = carService;
            _categoryService = categoryService;
            _paymentService = paymentService;
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


        [HttpGet("TripPlans")]
        public async Task<IActionResult> GetUpcomingTripPlans(){
            try{
                var result = await _tripPlanService.GetUpcomingTripPlansAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = "An error occurred while retrieving upcoming trip plans",
                    Details = ex.Message
                });
            }
        }

        [HttpGet("TripPlans/{id}")]
        public async Task<IActionResult> GetTripPlanById(int id)
        {   
            try{
                var tripPlan = await _tripPlanService.GetTripPlanByIdAsync(id);
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
                return StatusCode(500, new
                {
                    Error = "An error occurred while retrieving trip bookings",
                    Details = ex.Message
                });
            }
        }

        [HttpGet("TripBooking/{id}")]
        public async Task<IActionResult> GetTripBookingById(int id)
        {
            try
            {
                var tripBooking = await _tripBookingService.GetTripBookingByIdAsync(id);

                if (tripBooking == null) return NotFound(new { Error = $"Trip booking with ID {id} not found" });

                return Ok(tripBooking);
            }
            catch (Exception ex)
            {

                return StatusCode(500, new
                {
                    Error = $"An error occurred while retrieving trip booking with ID {id}",
                    Details = ex.Message
                });

            }
        }

        [HttpPost("TripBooking")]
        public async Task<IActionResult> CreateTripBooking([FromBody] CreateTripBookingDTO dto)
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
                var newTripBooking = await _tripBookingService.CreateTripBookingAsync(dto);
                return CreatedAtAction(nameof(GetTripBookingById), new { id = newTripBooking.Id }, newTripBooking);
            }

            catch(Exception ex){
                return StatusCode(500, new
                {
                    Error = "An error occurred while creating the trip booking",
                    Details = ex.Message
                });

            }
        }
        
        [HttpPut("TripBooking/{id}")]
        public async Task<IActionResult> UpdateTripBooking(int id, [FromBody] UpdateTripBookingDTO dto){
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
                await _tripBookingService.UpdateTripBookingAsync(dto);
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
                    Error = $"An error occurred while updating trip booking with ID {id}",
                    Details = ex.Message
                });
            }
        }
        //* Trip Bookings *//
        [HttpPut("CancelTripBooking/{id}")]
        public async Task<IActionResult> Cancel(int id) {
            try
            {
                await _tripBookingService.CancelTripBookingAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        // * CarBookings* //


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
                return StatusCode(500, new
                {
                    Error = "An error occurred while retrieving car bookings",
                    Details = ex.Message
                });
            }
        }


        [HttpGet("CarBooking/{id}")]
        public async Task<IActionResult> GetCarBookingById(int id)
        {
            try
            {
                var carBooking = await _carBookingService.GetCarBookingByIdAsync(id);

                if (carBooking == null) return NotFound(new { Error = $"Car booking with ID {id} not found" });

                return Ok(carBooking);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = $"An error occurred while retrieving car booking with ID {id}",
                    Details = ex.Message
                });
            }
        }

        [HttpPost("CarBooking")]
        public async Task<IActionResult> CreateCarBooking([FromBody] CreateCarBookingDTO dto)
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
                var newCarBooking = await _carBookingService.CreateCarBookingAsync(dto);
                return CreatedAtAction(nameof(GetCarBookingById), new { id = newCarBooking.Id }, newCarBooking);
            }
            catch (Exception ex)
            {

                return StatusCode(500, new
                {
                    Error = "An error occurred while creating the car booking",
                    Details = ex.Message
                });
            }
        }
        
        [HttpPut("CarBooking/{id}")]
        public async Task<IActionResult> UpdateCarBooking(int id, [FromBody] UpdateCarBookingDTO dto)
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
                await _carBookingService.UpdateCarBookingAsync(dto);
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
                    Error = $"An error occurred while updating car booking with ID {id}",
                    Details = ex.Message
                });

            }
        }

        // * Cars *  //
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

        [HttpPut("CancelCarBooking/{id}")]
        public async Task<IActionResult> CancelCarBooking(int id)
        {
            try
            {
                await _carBookingService.CancelCarBookingAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // * Categories* //
        [HttpGet("Categories")]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var result = await _categoryService.GetAllCategoriesAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = "Failed to retrieve categories",
                    Details = ex.Message
                });
            }
        }

        // * Payments * //

        /// <summary>
        /// Get all payments for the current customer
        /// </summary>
        [HttpGet("Payments")]
        public async Task<IActionResult> GetPayments([FromQuery] PaymentStatus? status = null)
        {
            try
            {
                var payments = status.HasValue
                    ? await _paymentService.GetPaymentsByStatusAsync(status.Value)
                    : await _paymentService.GetAllPaymentsAsync();
                
                return Ok(payments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = "An error occurred while retrieving payments",
                    Details = ex.Message
                });
            }
        }

        /// <summary>
        /// Get payment details by ID
        /// </summary>
        [HttpGet("Payments/{id}")]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            try
            {
                var payment = await _paymentService.GetPaymentByIdAsync(id);
                return Ok(payment);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Error = $"Payment with ID {id} not found" });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = $"An error occurred while retrieving payment with ID {id}",
                    Details = ex.Message
                });
            }
        }


        /// <summary>
        /// Get detailed payment information including transactions
        /// </summary>
        [HttpGet("Payments/{id}/Details")]
        public async Task<IActionResult> GetPaymentDetails(int id)
        {
            try
            {
                var details = await _paymentService.GetPaymentDetailsAsync(id);
                return Ok(details);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Error = $"Payment with ID {id} not found" });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = $"An error occurred while retrieving payment details for ID {id}",
                    Details = ex.Message
                });
            }
        }

        /// <summary>
        /// Process a payment
        /// </summary>
        [HttpPost("Payments/{id}/Process")]
        public async Task<IActionResult> ProcessPayment(int id, [FromBody] ProcessPaymentDTO processPaymentDto)
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

            // Ensure the payment ID in the URL matches the DTO
            if (id != processPaymentDto.PaymentId)
            {
                return BadRequest(new { Error = "Payment ID in URL does not match payment ID in request body" });
            }

            try
            {
                var result = await _paymentService.ProcessPaymentAsync(processPaymentDto);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Error = $"Payment with ID {id} not found" });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { Error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = "An error occurred while processing the payment",
                    Details = ex.Message
                });
            }
        }

        /// <summary>
        /// Cancel a payment (only for pending payments)
        /// </summary>
        [HttpPut("Payments/{id}/Cancel")]
        public async Task<IActionResult> CancelPayment(int id)
        {
            try
            {
                var result = await _paymentService.CancelPaymentAsync(id);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Error = $"Payment with ID {id} not found" });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { Error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = "An error occurred while cancelling the payment",
                    Details = ex.Message
                });
            }
        }
    }
}
