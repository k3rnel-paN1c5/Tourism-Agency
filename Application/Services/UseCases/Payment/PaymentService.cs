using Domain.Entities;
using Domain.Enums;
using Domain.IRepositories;
using Application.IServices.UseCases;
using Application.DTOs.Payment;

using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Http;
using AutoMapper;

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly ILogger<PaymentService> _logger;

    public PaymentService(
        IPaymentRepository paymentRepository,
        IBookingRepository bookingRepository,
        ILogger<PaymentService> logger)
    {
        _paymentRepository = paymentRepository;
        _bookingRepository = bookingRepository;
        _logger = logger;
    }

    public async Task<Payment> CreatePaymentAsync(int bookingId, decimal amountDue)
    {
        var booking = await _bookingRepository.GetByIdAsync(bookingId);
        if (booking == null)
            throw new KeyNotFoundException($"Booking {bookingId} not found.");

        var payment = new Payment
        {
            BookingId = bookingId,
            AmountDue = amountDue,
            AmountPaid = 0,
            Status = PaymentStatus.Pending,
            PaymentDate = null
        };

        await _paymentRepository.AddAsync(payment);
        await _paymentRepository.SaveAsync(); // Explicit save to ensure data consistency
        _logger.LogInformation($"Created payment {payment.Id} for booking {bookingId}.");
        return payment;
    }

    public async Task<Payment> GetPaymentByIdAsync(int paymentId)
    {
        var payment = await _paymentRepository.GetByIdAsync(paymentId);
        if (payment == null)
            throw new KeyNotFoundException($"Payment {paymentId} not found.");
        return payment;
    }

    public async Task<Payment> GetPaymentByBookingIdAsync(int bookingId)
    {
        var payment = await _paymentRepository.GetByBookingIdAsync(bookingId);
        if (payment == null)
            throw new KeyNotFoundException($"No payment found for booking {bookingId}.");
        return payment;
    }

    public async Task<Payment> UpdatePaymentStatusAsync(int paymentId, PaymentStatus status)
    {
        var payment = await GetPaymentByIdAsync(paymentId);

        if (payment.Status == PaymentStatus.Refunded)
            throw new InvalidOperationException("Refunded payments cannot be modified.");

        payment.Status = status;

        if (status == PaymentStatus.Paid)
            payment.PaymentDate = DateTime.UtcNow;

        _paymentRepository.Update(payment);
        await _paymentRepository.SaveAsync();
        _logger.LogInformation($"Updated payment {paymentId} status to {status}.");
        return payment;
    }

    public async Task<Payment> ProcessRefundAsync(int paymentId, decimal refundAmount, string reason)
    {
        var payment = await GetPaymentByIdAsync(paymentId);

        if (payment.Status != PaymentStatus.Paid)
            throw new InvalidOperationException("Only paid payments can be refunded.");

        if (refundAmount <= 0 || refundAmount > payment.AmountPaid)
            throw new ArgumentException($"Invalid refund amount: {refundAmount}");

        payment.AmountPaid -= refundAmount;
        payment.Notes = $"REFUND: {reason}";

        if (payment.AmountPaid == 0)
            payment.Status = PaymentStatus.Refunded;
        else
            payment.Status = PaymentStatus.PartiallyRefunded;

        _paymentRepository.Update(payment);
        await _paymentRepository.SaveAsync();
        _logger.LogInformation($"Processed refund of {refundAmount} for payment {paymentId}.");
        return payment;
    }

    public async Task<IEnumerable<Payment>> GetPaymentsByStatusAsync(PaymentStatus status)
    {
        return await _paymentRepository.GetByStatusAsync(status);
    }
}