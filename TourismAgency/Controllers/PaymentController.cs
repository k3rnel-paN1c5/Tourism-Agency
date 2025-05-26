using Microsoft.AspNetCore.Mvc;
using Application.IServices.UseCases;
using Domain.Enums;
using Application.DTOs.Payment;
using Microsoft.AspNetCore.Authorization;

namespace TourismAgency.Controllers
{
    [ApiController]
    [Route("api/payments")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(
            IPaymentService paymentService,
            ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        // [HttpPost("CreateP")]
        // public async Task<ActionResult<ReturnPaymentDTO>> CreatePayment([FromBody] CreatePaymentDTO createPaymentDTO)
        // {
        //     var payment = await _paymentService.CreatePaymentAsync(createPaymentDTO);
        //     return Ok(payment);
// 
        // }


        /// Get payment details by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ReturnPaymentDTO>> GetPayment(int id)
        {
            try
            {
                var payment = await _paymentService.GetPaymentByIdAsync(id);
                return Ok(payment);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving payment {PaymentId}", id);
                return Problem("An error occurred while retrieving payment details.",
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }
        

        /// Get payment by booking ID
        [HttpGet("bookings/{bookingId}")]
        public async Task<ActionResult<ReturnPaymentDTO>> GetPaymentByBooking(int bookingId)
        {
            try
            {
                var payment = await _paymentService.GetPaymentByBookingIdAsync(bookingId);
                return Ok(payment);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving payment for booking {BookingId}", bookingId);
                return Problem("An error occurred while retrieving payment details.", 
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }


        /// Update payment status
        [HttpPatch("{id}/status")]
        public async Task<ActionResult<ReturnPaymentDTO>> UpdateStatus(int id,[FromBody] UpdatePaymentStatusDTO statusDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var payment = await _paymentService.UpdatePaymentStatusAsync(statusDto);
                return Ok(payment);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating status for payment {PaymentId}", id);
                return Problem("An error occurred while updating payment status.",
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        
        /// Process a refund
        [HttpPost("{id}/refunds")]
        public async Task<ActionResult<ReturnPaymentDTO>> ProcessRefund([FromBody] ProcessRefundDTO refundDto)
        {
            //Task<ReturnPaymentDTO> ProcessRefundAsync(ProcessRefundDTO refundDto)
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var payment = await _paymentService.ProcessRefundAsync(refundDto);
                return Ok(payment);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(nameof(refundDto.Amount), ex.Message);
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing refund for payment {PaymentId}", refundDto.PaymentId);
                return Problem("An error occurred while processing refund.", 
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        
        /// Get payments by status
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReturnPaymentDTO>>> GetPaymentsByStatus([FromQuery] PaymentStatus? status = null)
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
                _logger.LogError(ex, "Error retrieving payments by status {Status}", status);
                return Problem("An error occurred while retrieving payments.", 
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        
        /// Get detailed payment information
        [HttpGet("{id}/details")]
        public async Task<ActionResult<PaymentDetailsDTO>> GetPaymentDetails(int id)
        {
            try
            {
                var details = await _paymentService.GetPaymentDetailsAsync(id);
                return Ok(details);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving details for payment {PaymentId}", id);
                return Problem("An error occurred while retrieving payment details.", 
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}