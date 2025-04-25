using Microsoft.AspNetCore.Mvc;
using Application.IServices.UseCases;
using Application.DTOs.CarBooking;



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

            // Handle any exceptions that occur during the save operation
            try{
                // Save the CarBooking object to the database
                await _carBookingService.CreateBookingAsync(createCarBookingDTO);
                return RedirectToAction("Success");
                
            }
            catch (InvalidOperationException ex)
            {
                 // Add the error to ModelState to display in the view
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch (Exception ex)
            {
                // Handle other unexpected errors
                ModelState.AddModelError(string.Empty, "An error occurred. Please try again." + ex.Message);
            }
            return View(createCarBookingDTO);

        }

        // GET: CarBooking/Success
        public IActionResult Success()
        {
            return View();
        }
    }
}