using Microsoft.AspNetCore.Mvc;
using Application.IServices.UseCases;
using Application.DTOs.Payment;

namespace TourismAgency.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly ILogger<PaymentsController> _logger;
        private readonly IPaymentService _paymentService;

        public PaymentsController(
            ILogger<PaymentsController> logger,
            IPaymentService paymentService)
        {
            _logger = logger;
            _paymentService = paymentService;
        }

        // GET: Payments/Create?bookingId=1&amountDue=100
        public IActionResult Create(long bookingId, double amountDue)
        {
            try
            {
                var payment = _paymentService.CreatePayment(bookingId, amountDue);
                return View(payment); // Render a view to show payment details
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create payment");
                return View("Error", new ErrorViewModel 
                { 
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorMessage = ex.Message 
                });
            }
        }

        // GET: Payments/Details/5
        public IActionResult Details(long id)
        {
            try
            {
                var payment = _paymentService.GetPaymentById(id);
                return View(payment); // Render a view with payment details
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Payment {id} not found");
                return View("Error", new ErrorViewModel 
                { 
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorMessage = "Payment not found." 
                });
            }
        }

       

        // GET: Payments/UpdateStatus/5?status=Paid
        public IActionResult UpdateStatus(long id, PaymentStatus status)
        {
            try
            {
                var payment = _paymentService.UpdatePaymentStatus(id, status);
                return RedirectToAction(nameof(Details), new { id }); // Redirect to details after update
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Status update failed for payment {id}");
                return View("Error", new ErrorViewModel 
                { 
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorMessage = ex.Message 
                });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel 
            { 
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier 
            });
        }
    }
}