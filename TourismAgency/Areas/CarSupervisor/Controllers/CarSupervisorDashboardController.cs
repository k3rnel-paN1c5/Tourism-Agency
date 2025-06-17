using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.IServices.UseCases;
using Application.DTOs.Car;
using Application.DTOs.Category;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.EntityFrameworkCore;
using Application.Services.UseCases;

namespace TourismAgency.Areas.CarSupervisor.Controllers
{
    [Area("CarSupervisor")]
    [Route("api/[area]/[controller]")]
    [Authorize(Roles = "CarSupervisor,Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class CarSupervisorDashboardController : ControllerBase
    {
        private readonly ICarService _carService;
        private readonly ICategoryService _categoryService;
        private readonly ICarBookingService _carBookingService;

        public CarSupervisorDashboardController(ICarService carService, ICategoryService categoryService, ICarBookingService carBookingService)
        {
            _carService = carService;
            _categoryService = categoryService;
            _carBookingService = carBookingService;
        }

        [HttpGet] // No route parameter
        public IActionResult GetDefault()
        {
            return Ok(new { Message = "Welcome to the Car Supervisor Dashboard" });
        }

        //* Category *//
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
       
        [HttpGet("Categories/{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                var cat = await _categoryService.GetCategoryByIdAsync(id);
                if (cat == null) return NotFound(new { Error = $"Category with ID {id} not found" });
                return Ok(cat);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = $"An error occurred while retrieving category with ID {id}",
                    Details = ex.Message
                });
            }
        }
       
        [HttpPost("Categories")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDTO dto)
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
                var newCat = await _categoryService.CreateCategoryAsync(dto);
                return CreatedAtAction(nameof(GetCategoryById), new { id = newCat.Id }, newCat);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = "Failed to create category",
                    Details = ex.Message
                });
            }

        }

        [HttpPut("Categories/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDTO dto)
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
                await _categoryService.UpdateCategoryAsync(dto);
                return Ok(dto);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new { Error = e.Message });

            }
            catch (DbUpdateException e)
            {
                return Conflict(new { Error = e.Message });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = $"An error occurred while updating category with ID {id}",
                    Details = ex.Message
                });
            }
        }

        [HttpDelete("Categories/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
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
                await _categoryService.DeleteCategoryAsync(id);
                return Ok($"Deleted category with id {id}");
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new { Error = e.Message });

            }
            catch (DbUpdateException e)
            {
                return Conflict(new { Error = e.Message });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = $"An error occurred while Deleting Category with ID {id}",
                    Details = ex.Message
                });
            }
        }

        //* Cars *//

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

        [HttpGet("Cars/{id}")]
        public async Task<IActionResult> GetCarById(int id)
        {
            try
            {
                var car = await _carService.GetCarByIdAsync(id);
                if (car == null) return NotFound(new { Error = $"Car with ID {id} not found" }); 
                return Ok(car);
            }
            catch (Exception ex) 
            {
                return StatusCode(500, new
                {
                    Error = $"An error occurred while retrieving car with ID {id}",
                    Details = ex.Message
                });
            }
        }

        [HttpPost("Cars")]
        public async Task<IActionResult> CreateCar([FromBody] CreateCarDTO dto)
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

                var newCar = await _carService.CreateCarAsync(dto);
                return CreatedAtAction(nameof(GetCarById), new { id = newCar.Id }, newCar);
            }
            catch(Exception ex)
            {
                return StatusCode(500, new{
                    Error = "Failed to create car",
                    Details = ex.Message
                });
            }
        }

        [HttpPut("Cars/{id}")]
        public async Task<IActionResult> UpdateCar(int id, [FromBody] UpdateCarDTO dto)
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
                await _carService.UpdateCarAsync(dto);
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
                    Error = $"An error occurred while updating car with ID {id}",
                    Details = ex.Message
                });
            }
        }

        [HttpDelete("Cars/{id}")]
        public async Task<IActionResult> DeleteCar(int id)
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
                await _carService.DeleteCarAsync(id);
                return Ok($"Deleted A car with id {id}");
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new { Error = e.Message });

            }
            catch (DbUpdateException e)
            {
                return Conflict(new { Error = e.Message });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = $"An error occurred while Deleting car with ID {id}",
                    Details = ex.Message
                });
            }
        }

        [HttpGet("AvailableCars")]
        public async Task<IActionResult> GetAvailableCarsAsync(DateTime startDate,DateTime endDate)
        {
            try
            {
                var result = await _carService.GetAvailableCarsAsync(startDate , endDate);
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



        //* CarBooking *//

        [HttpGet("carbooking")]
        public async Task<IActionResult> GetCarBookings()
        {
            try
            {
                var result = await _carBookingService.GetAllCarBookingsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new{
                    Error = "Failed to retrieve car bookings",
                    Details = ex.Message
                });
            }


        }

    }
}
