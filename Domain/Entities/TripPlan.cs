﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public partial class TripPlan
    {
        public TripPlan()
        {
            Bookings = new HashSet<TripBooking>();
            PlanCars = new HashSet<TripPlanCar>();
        }
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("tripId")]
        [ForeignKey("Trip")]
        public int TripId { get; set; }

        [Required]
        [Column("regionId")]
        [ForeignKey("Region")]
        public int RegionId { get; set; }

        [Required]
        [Column("startDate", TypeName = "datetime2(7)")]
        public DateTime StartDate { get; set; }

        [Required]
        [Column("endDate", TypeName = "datetime2(7)")]
        public DateTime EndtDate { get; set; }

        // TimeSpan is a structure in C# that represents a time interval.
        // Can be stored as nvaechar in the database
        [Column("duration", TypeName = "nvarchar(50)")]
        public TimeSpan Duration { get; set; } = TimeSpan.FromHours(1);


        [Column("includedServices", TypeName = "nvarchar(200)")]
        public string IncludedServices { get; set; } = string.Empty;

        [Column("stops", TypeName = "nvarchar(200)")]
        public string Stops { get; set; } = string.Empty;

        [Column("mealsPlan", TypeName = "nvarchar(200)")]
        public string MealsPlan { get; set; } = string.Empty;

        [Column("hotelStays", TypeName = "nvarchar(200)")]
        public string HotelStays { get; set; } = string.Empty;

        // Navigation Properties

        public Region? Region { get; set; }
        public Trip? Trip { get; set; }
        public ICollection<TripBooking> Bookings { get; set; }
        public ICollection<TripPlanCar> PlanCars { get; set; }



    }
}
