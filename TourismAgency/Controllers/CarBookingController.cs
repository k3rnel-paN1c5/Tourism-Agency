using Microsoft.AspNetCore.Mvc;
using DataAccess.Entities;
using DTO.CarBooking;
using DataAccess.Contexts;
using BusinessLogic.IServices;



namespace TourismAgency.Controllers
{
    public class CarBookingController : Controller
    {
        private readonly ICarBookingService _carBookingService;

        public CarBookingController(ICarBookingService carBookingService)
        {
            _carBookingService = carBookingService; // Inject the  service
        }
        // GET: CarBooking/Create
        public IActionResult Create()
        {
            // Initialize a new CarBooking object with an empty Booking
            var createCarBookingDTO = new CreateCarBookingDTO();
            return View(createCarBookingDTO);
        }

        // POST: CarBooking/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCarBookingDTO createCarBookingDTO)
        {
            // If the model state is invalid, return the view with validation errors
            if (!ModelState.IsValid)
                return View(createCarBookingDTO);

            // Save the CarBooking object to the database
            await _carBookingService.CreateBookingAsync(createCarBookingDTO);

            return RedirectToAction("Success");
            

        }

        // GET: CarBooking/Success
        public IActionResult Success()
        {
            return View();
        }
    }
}