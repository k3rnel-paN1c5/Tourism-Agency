using Domain.Enums;
namespace Application.DTOs.Payment{
    public class PaymentDetailsDTO
{
    public int Id { get; set; }
    public int BookingId { get; set; }
    public decimal AmountDue { get; set; }
    public decimal AmountPaid { get; set; }
    public PaymentStatus Status { get; set; }
    public DateTime? PaymentDate { get; set; }
}
}
