namespace Application.DTOs.Payment{
    public class CreatePaymentDTO
{
    public int BookingId { get; set; }
    public decimal AmountDue { get; set; }
}
}