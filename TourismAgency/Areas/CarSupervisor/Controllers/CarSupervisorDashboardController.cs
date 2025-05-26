using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.IServices.UseCases;
using Application.DTOs.Car;
using Application.DTOs.Category;

namespace TourismAgency.Areas.CarSupervisor.Controllers
{
    [Area("CarSupervisor")]
    [Route("api/[controller]")]
    [Authorize(Roles = "CarSupervisor,Admin")]
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
        [HttpGet("category")]
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
       
        [HttpGet("category/{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                var cat = await _categoryService.GetCategoryByIdAsync(id);
                if (cat == null) return NotFound();
                return Ok(cat);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = $"Failed to retrieve category with ID {id}",
                    Details = ex.Message
                });
            }
        }
       
        [HttpPost("category")]
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
                    Error = "Failed to retrieve cars",
                    Details = ex.Message
                });
            }

        }

        [HttpGet("car/{id}")]
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
                    Error = $"Failed to retrieve car with ID {id}",
                    Details = ex.Message
                });
            }
        }

        [HttpPost("Car")]
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

        //* Cars *//
        
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
