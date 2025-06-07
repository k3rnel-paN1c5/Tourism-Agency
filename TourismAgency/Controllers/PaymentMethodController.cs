using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.IServices.UseCases;
using DTO.PaymentMethod;

namespace TourismAgency.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentMethodController : ControllerBase
    {
        private readonly IPaymentMethodService _paymentMethodService;
        private readonly ILogger<PaymentMethodController> _logger;

        public PaymentMethodController(
            IPaymentMethodService paymentMethodService,
            ILogger<PaymentMethodController> logger)
        {
            _paymentMethodService = paymentMethodService;
            _logger = logger;
        }

        /// <summary>
        /// Get all active payment methods (public access for customers)
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReturnPaymentMethodDTO>>> GetActivePaymentMethods()
        {
            try
            {
                var paymentMethods = await _paymentMethodService.GetActivePaymentMethodsAsync();
                return Ok(paymentMethods);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving active payment methods");
                return Problem("An error occurred while retrieving payment methods.",
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get all payment methods including inactive ones (admin only)
        /// </summary>
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<ReturnPaymentMethodDTO>>> GetAllPaymentMethods()
        {
            try
            {
                var paymentMethods = await _paymentMethodService.GetAllPaymentMethodsAsync();
                return Ok(paymentMethods);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all payment methods");
                return Problem("An error occurred while retrieving payment methods.",
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get payment method by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ReturnPaymentMethodDTO>> GetPaymentMethodById(int id)
        {
            try
            {
                var paymentMethod = await _paymentMethodService.GetPaymentMethodByIdAsync(id);
                return Ok(paymentMethod);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Payment method with ID {id} not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving payment method {PaymentMethodId}", id);
                return Problem("An error occurred while retrieving payment method details.",
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Create a new payment method (admin only)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ReturnPaymentMethodDTO>> CreatePaymentMethod([FromBody] CreatePaymentMethodDTO createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var paymentMethod = await _paymentMethodService.CreatePaymentMethodAsync(createDto);
                return CreatedAtAction(
                    nameof(GetPaymentMethodById),
                    new { id = paymentMethod.Id },
                    paymentMethod);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(nameof(createDto.Method), ex.Message);
                return BadRequest(ModelState);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating payment method");
                return Problem("An error occurred while creating the payment method.",
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Update an existing payment method (admin only)
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ReturnPaymentMethodDTO>> UpdatePaymentMethod(int id, [FromBody] UpdatePaymentMethodDTO updateDto)
        {
            try
            {
                if (id != updateDto.Id)
                {
                    return BadRequest("ID in URL does not match ID in request body");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var paymentMethod = await _paymentMethodService.UpdatePaymentMethodAsync(updateDto);
                return Ok(paymentMethod);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Payment method with ID {id} not found");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(nameof(updateDto.Method), ex.Message);
                return BadRequest(ModelState);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating payment method {PaymentMethodId}", id);
                return Problem("An error occurred while updating the payment method.",
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Delete a payment method (admin only)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePaymentMethod(int id)
        {
            try
            {
                var result = await _paymentMethodService.DeletePaymentMethodAsync(id);
                if (!result)
                {
                    return NotFound($"Payment method with ID {id} not found");
                }

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting payment method {PaymentMethodId}", id);
                return Problem("An error occurred while deleting the payment method.",
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Toggle payment method active status (admin only)
        /// </summary>
        [HttpPatch("{id}/toggle")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ReturnPaymentMethodDTO>> TogglePaymentMethodStatus(int id)
        {
            try
            {
                var paymentMethod = await _paymentMethodService.TogglePaymentMethodStatusAsync(id);
                return Ok(paymentMethod);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Payment method with ID {id} not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling payment method status {PaymentMethodId}", id);
                return Problem("An error occurred while toggling payment method status.",
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}