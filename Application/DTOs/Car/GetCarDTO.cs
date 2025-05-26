namespace Application.DTOs.Car
{
    public class GetCarDTO
    {
        public int Id { get; set; }
        public string Model { get; set; } = string.Empty;
        public int Seats { get; set; }
        public string Color { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public decimal Pph { get; set; } // Price per hour
        public decimal Ppd { get; set; } // Price per day
        public decimal Mbw { get; set; }// Maximum baggage weight
        public int CategoryId { get; set; }
    }
}
