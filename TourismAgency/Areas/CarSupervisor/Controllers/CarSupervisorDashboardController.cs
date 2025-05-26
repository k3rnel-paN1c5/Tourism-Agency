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
            var result = await _categoryService.GetAllCategoriesAsync();
            return Ok(result);
        }
        [HttpGet("category/{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var cat = await _categoryService.GetCategoryByIdAsync(id);
            if (cat == null) return NotFound();
            return Ok(cat);
        }
        [HttpPost("category")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var newCat = await _categoryService.CreateCategoryAsync(dto);
            return CreatedAtAction(nameof(GetCategoryById), new { id = newCat.Id }, newCat);

        }


        //* Cars *//

        [HttpGet("cars")]
        public async Task<IActionResult> GetCars()
        {
            var result = await _carService.GetAllCarsAsync();
            return Ok(result);
        }
        [HttpGet("car/{id}")]
        public async Task<IActionResult> GetCarById(int id)
        {
            var car = await _carService.GetCarByIdAsync(id);
            if (car == null) return NotFound();
            return Ok(car);
        }
        [HttpPost("Car")]
        public async Task<IActionResult> CreateCar([FromBody] CreateCarDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var newCar = await _carService.CreateCarAsync(dto);
            return CreatedAtAction(nameof(GetCarById), new { id = newCar.Id }, newCar);
        }

        //* Cars *//
        
        [HttpGet("carbooking")]
        public async Task<IActionResult> GetCarBookings()
        {
            var result = await _carBookingService.GetAllCarBookingsAsync();
            return Ok(result);
        }

    }
}
