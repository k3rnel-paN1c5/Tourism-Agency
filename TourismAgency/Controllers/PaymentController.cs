using Microsoft.AspNetCore.Mvc;
using Application.IServices.UseCases;
using Domain.Enums;
using Application.DTOs.Payment;
using Application.DTOs.User;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace TourismAgency.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger; //???????????

        public PaymentController(
            IPaymentService paymentService,
            ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        // GET: Payment/Create/{bookingId}
        public IActionResult Create(int bookingId)
        {
            var createPaymentDto = new CreatePaymentDTO { BookingId = bookingId };
            return View(createPaymentDto);
        }

        // POST: Payment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePaymentDTO createPaymentDto)
        {
            if (!ModelState.IsValid)
                return View(createPaymentDto);

            try
            {
                var payment = await _paymentService.CreatePaymentAsync(
                    createPaymentDto.BookingId, 
                    createPaymentDto.AmountDue);
                
                return RedirectToAction("Details", new { id = payment.Id });
            }
            catch (KeyNotFoundException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(createPaymentDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating payment");
                ModelState.AddModelError(string.Empty, "An error occurred while creating the payment.");
                return View(createPaymentDto);
            }
        }

        // GET: Payment/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var payment = await _paymentService.GetPaymentByIdAsync(id);
                var paymentDto = new PaymentDetailsDTO
                {
                    Id = payment.Id,
                    BookingId = payment.BookingId,
                    AmountDue = payment.AmountDue,
                    AmountPaid = payment.AmountPaid,
                    Status = payment.Status,
                    PaymentDate = payment.PaymentDate
                };
                
                return View(paymentDto);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // GET: Payment/ByBooking/{bookingId}
        public async Task<IActionResult> ByBooking(int bookingId)
        {
            try
            {
                var payment = await _paymentService.GetPaymentByBookingIdAsync(bookingId);
                return RedirectToAction("Details", new { id = payment.Id });
            }
            catch (KeyNotFoundException)
            {
                return View("NoPayment", bookingId);
            }
        }

        // GET: Payment/UpdateStatus/{id}
        public async Task<IActionResult> UpdateStatus(int id)
        {
            try
            {
                var payment = await _paymentService.GetPaymentByIdAsync(id);
                var updateDto = new UpdatePaymentStatusDTO
                {
                    PaymentId = payment.Id,
                    CurrentStatus = payment.Status
                };
                
                return View(updateDto);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // POST: Payment/UpdateStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(UpdatePaymentStatusDTO updateDto)
        {
            if (!ModelState.IsValid)
                return View(updateDto);

            try
            {
                var payment = await _paymentService.UpdatePaymentStatusAsync(
                    updateDto.PaymentId, 
                    updateDto.NewStatus);
                
                return RedirectToAction("Details", new { id = payment.Id });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(updateDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating payment status");
                ModelState.AddModelError(string.Empty, "An error occurred while updating payment status.");
                return View(updateDto);
            }
        }

        // GET: Payment/ProcessRefund/{id}
        public async Task<IActionResult> ProcessRefund(int id)
        {
            try
            {
                var payment = await _paymentService.GetPaymentByIdAsync(id);
                var refundDto = new ProcessRefundDTO
                {
                    PaymentId = payment.Id,
                    MaxRefundAmount = payment.AmountPaid
                };
                
                return View(refundDto);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // POST: Payment/ProcessRefund
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessRefund(ProcessRefundDTO refundDto)
        {
            if (!ModelState.IsValid)
                return View(refundDto);

            try
            {
                var payment = await _paymentService.ProcessRefundAsync(
                    refundDto.PaymentId,
                    refundDto.Amount,
                    refundDto.Reason);
                
                return RedirectToAction("Details", new { id = payment.Id });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(refundDto);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(nameof(refundDto.Amount), ex.Message);
                return View(refundDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing refund");
                ModelState.AddModelError(string.Empty, "An error occurred while processing the refund.");
                return View(refundDto);
            }
        }

        // GET: Payment/ByStatus/{status}
        public async Task<IActionResult> ByStatus(PaymentStatus status)
        {
            try
            {
                var payments = await _paymentService.GetPaymentsByStatusAsync(status);
                var paymentDtos = new List<PaymentDetailsDTO>();
                
                foreach (var payment in payments)
                {
                    paymentDtos.Add(new PaymentDetailsDTO
                    {
                        Id = payment.Id,
                        BookingId = payment.BookingId,
                        AmountDue = payment.AmountDue,
                        AmountPaid = payment.AmountPaid,
                        Status = payment.Status,
                        PaymentDate = payment.PaymentDate
                    });
                }
                
                return View(paymentDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving payments by status");
                return View("Error");
            }
        }
    }
}