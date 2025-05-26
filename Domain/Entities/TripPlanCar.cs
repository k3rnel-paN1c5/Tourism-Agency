using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public partial class TripPlanCar
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("tripPlanId")]
        [ForeignKey("TripPlan")]
        public int TripPlanId { get; set; }

        [Required]
        [Column("carId")]
        [ForeignKey("Car")]
        public int CarId { get; set; }

        [Required]
        [Column("price", TypeName = "decimal(16,2)")]
        public decimal Price { get; set; }

        // Navigation Properties
        public Car? Car { get; set; }
        public TripPlan? TripPlan { get; set; }
    }
}
