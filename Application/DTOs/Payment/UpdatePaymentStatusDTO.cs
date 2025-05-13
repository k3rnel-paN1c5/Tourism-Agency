public class UpdatePaymentStatusDTO
{
    public int PaymentId { get; set; }
    public PaymentStatus CurrentStatus { get; set; }
    public PaymentStatus NewStatus { get; set; }
}