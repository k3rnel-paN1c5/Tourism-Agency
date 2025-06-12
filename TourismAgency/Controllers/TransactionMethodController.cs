using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.IServices.UseCases;
using Application.DTOs.TransactionMethod;

namespace TourismAgency.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionMethodController : ControllerBase
    {
        private readonly ITransactionMethodService _TransactionMethodService;
        private readonly ILogger<TransactionMethodController> _logger;

        public TransactionMethodController(
            ITransactionMethodService TransactionMethodService,
            ILogger<TransactionMethodController> logger)
        {
            _TransactionMethodService = TransactionMethodService;
            _logger = logger;
        }

        /// <summary>
        /// Get all active Transaction Methods (public access for customers)
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReturnTransactionMethodDTO>>> GetActiveTransactionMethods()
        {
            try
            {
                var TransactionMethods = await _TransactionMethodService.GetActiveTransactionMethodsAsync();
                return Ok(TransactionMethods);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving active Transaction Methods");
                return Problem("An error occurred while retrieving Transaction Methods.",
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get all Transaction Methods including inactive ones (admin only)
        /// </summary>
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<ReturnTransactionMethodDTO>>> GetAllTransactionMethods()
        {
            try
            {
                var TransactionMethods = await _TransactionMethodService.GetAllTransactionMethodsAsync();
                return Ok(TransactionMethods);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all Transaction Methods");
                return Problem("An error occurred while retrieving Transaction Methods.",
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get Transaction Method by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ReturnTransactionMethodDTO>> GetTransactionMethodById(int id)
        {
            try
            {
                var TransactionMethod = await _TransactionMethodService.GetTransactionMethodByIdAsync(id);
                return Ok(TransactionMethod);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Transaction Method with ID {id} not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Transaction Method {TransactionMethodId}", id);
                return Problem("An error occurred while retrieving Transaction Method details.",
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Create a new Transaction Method (admin only)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ReturnTransactionMethodDTO>> CreateTransactionMethod([FromBody] CreateTransactionMethodDTO createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var TransactionMethod = await _TransactionMethodService.CreateTransactionMethodAsync(createDto);
                return CreatedAtAction(
                    nameof(GetTransactionMethodById),
                    new { id = TransactionMethod.Id },
                    TransactionMethod);
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
                _logger.LogError(ex, "Error creating Transaction Method");
                return Problem("An error occurred while creating the Transaction Method.",
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Update an existing Transaction Method (admin only)
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ReturnTransactionMethodDTO>> UpdateTransactionMethod(int id, [FromBody] UpdateTransactionMethodDTO updateDto)
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

                var TransactionMethod = await _TransactionMethodService.UpdateTransactionMethodAsync(updateDto);
                return Ok(TransactionMethod);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Transaction Method with ID {id} not found");
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
                _logger.LogError(ex, "Error updating Transaction Method {TransactionMethodId}", id);
                return Problem("An error occurred while updating the Transaction Method.",
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Delete a Transaction Method (admin only)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTransactionMethod(int id)
        {
            try
            {
                var result = await _TransactionMethodService.DeleteTransactionMethodAsync(id);
                if (!result)
                {
                    return NotFound($"Transaction Method with ID {id} not found");
                }

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Transaction Method {TransactionMethodId}", id);
                return Problem("An error occurred while deleting the Transaction Method.",
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Toggle Transaction Method active status (admin only)
        /// </summary>
        [HttpPatch("{id}/toggle")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ReturnTransactionMethodDTO>> ToggleTransactionMethodStatus(int id)
        {
            try
            {
                var TransactionMethod = await _TransactionMethodService.ToggleTransactionMethodStatusAsync(id);
                return Ok(TransactionMethod);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Transaction Method with ID {id} not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling Transaction Method status {TransactionMethodId}", id);
                return Problem("An error occurred while toggling Transaction Method status.",
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}