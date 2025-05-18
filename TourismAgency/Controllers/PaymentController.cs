using Microsoft.AspNetCore.Mvc;
using Application.IServices.UseCases;
using Domain.Enums;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Application.DTOs.Payment;

namespace TourismAgency.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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

        
        /// Creates a new payment
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PaymentDetailsDTO>> CreatePayment([FromBody] CreatePaymentDTO createPaymentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var payment = await _paymentService.CreatePaymentAsync(
                    createPaymentDto.BookingId, 
                    createPaymentDto.AmountDue);
                
                var paymentDto = MapToDetailsDto(payment);
                return CreatedAtAction(nameof(GetPayment), new { id = payment.Id }, paymentDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating payment");
                return Problem("An error occurred while creating the payment.", statusCode: 500);
            }
        }

        
        /// Gets payment details by ID
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PaymentDetailsDTO>> GetPayment(int id)
        {
            try
            {
                var payment = await _paymentService.GetPaymentByIdAsync(id);
                return Ok(MapToDetailsDto(payment));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        
        /// Gets payment by booking ID
        [HttpGet("by-booking/{bookingId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PaymentDetailsDTO>> GetPaymentByBooking(int bookingId)
        {
            try
            {
                var payment = await _paymentService.GetPaymentByBookingIdAsync(bookingId);
                return Ok(MapToDetailsDto(payment));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        
        /// Updates payment status
        [HttpPatch("{id}/status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PaymentDetailsDTO>> UpdatePaymentStatus(int id, [FromBody] UpdatePaymentStatusDTO updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var payment = await _paymentService.UpdatePaymentStatusAsync(id, updateDto.NewStatus);
                return Ok(MapToDetailsDto(payment));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating payment status");
                return Problem("An error occurred while updating payment status.", statusCode: 500);
            }
        }

        
        /// Processes a refund
        [HttpPost("{id}/refund")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PaymentDetailsDTO>> ProcessRefund(int id, [FromBody] ProcessRefundDTO refundDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var payment = await _paymentService.ProcessRefundAsync(
                    id,
                    refundDto.Amount,
                    refundDto.Reason);
                
                return Ok(MapToDetailsDto(payment));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(nameof(refundDto.Amount), ex.Message);
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing refund");
                return Problem("An error occurred while processing the refund.", statusCode: 500);
            }
        }

        
        /// Gets payments by status
        [HttpGet("by-status/{status}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PaymentDetailsDTO>>> GetPaymentsByStatus(PaymentStatus status)
        {
            try
            {
                var payments = await _paymentService.GetPaymentsByStatusAsync(status);
                var paymentDtos = payments.Select(MapToDetailsDto).ToList();
                return Ok(paymentDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving payments by status");
                return Problem("An error occurred while retrieving payments.", statusCode: 500);
            }
        }

        private PaymentDetailsDTO MapToDetailsDto(Payment payment)
        {
            return new PaymentDetailsDTO
            {
                Id = payment.Id,
                BookingId = payment.BookingId,
                AmountDue = payment.AmountDue,
                AmountPaid = payment.AmountPaid,
                Status = payment.Status,
                PaymentDate = payment.PaymentDate
            };
        }
    }
}