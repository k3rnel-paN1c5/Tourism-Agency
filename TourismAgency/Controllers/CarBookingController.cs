using Microsoft.AspNetCore.Mvc;
using DataAccess.Entities;

using DataAccess.Contexts;



namespace TourismAgency.Controllers
{
    public class CarBookingController : Controller
    {
        private readonly TourismAgencyDbContext _context;

        public CarBookingController(TourismAgencyDbContext context)
        {
            _context = context; // Inject the database context
        }
        // GET: CarBooking/Create
        public IActionResult Create()
        {
            // Initialize a new CarBooking object with an empty Booking
            var carBooking = new CarBooking
            {
                Booking = new Booking() // Initialize the Booking navigation property
            };
            return View(carBooking);
        }

        // POST: CarBooking/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CarBooking carBooking)
        {
            if (ModelState.IsValid)
            {
                // Save the CarBooking object to the database
                _context.CarBookings.Add(carBooking);
                _context.SaveChanges();

                // Redirect to the success page
                return RedirectToAction("Success");
            }

            // If the model state is invalid, return the view with validation errors
            return View(carBooking);
        }

        // GET: CarBooking/Success
        public IActionResult Success()
        {
            return View();
        }
    }
}