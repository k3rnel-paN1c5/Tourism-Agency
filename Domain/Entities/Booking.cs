using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain.Entities;

/// <summary>
/// Represents a booking made by a customer for either a car or a trip.
/// </summary>
public partial class Booking
{
    /// <summary>
    /// Unique identifier for the booking.
    /// </summary>
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// A value indicating whether the booking is for a car (false) or a trip (true).
    /// </summary>
    [Required]
    [Column("bookingType")]
    public bool BookingType { get; set; }

    /// <summary>
    /// Start date and time of the booking.
    /// </summary>
    [Required]
    [Column("startDate", TypeName = "datetime2(7)")]
    public DateTime StartDate { get; set; }

    /// <summary>
    /// End date and time of the booking.
    /// </summary>
    [Required]
    [Column("endDate", TypeName = "datetime2(7)")]
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Current status of the booking (e.g., Pending, Confirmed, Cancelled).
    /// </summary>
    [Required]
    [Column("status")]
    [EnumDataType(typeof(BookingStatus))]
    public BookingStatus Status { get; set; } = BookingStatus.Pending;

    /// <summary>
    /// Number of passengers included in the booking.
    /// </summary>
    [Required]
    [Column("numOfPassengers")]
    public int NumOfPassengers { get; set; }

    /// <summary>
    /// Foreign key for the customer who made the booking.
    /// </summary>
    [Required]
    [Column("customerId")]
    [ForeignKey("Customer")]
    public string? CustomerId { get; set; }

    /// <summary>
    /// Foreign key for the employee who handled the booking, if applicable.
    /// </summary>
    [Column("employeeId")]
    [ForeignKey("Employee")]
    public string? EmployeeId { get; set; }


    // Navigation Properties
    /// <summary>
    /// Navigation property to the Employee associated with this booking.
    /// </summary>
    public Employee? Employee { get; set; }

    /// <summary>
    /// Navigation property to the Customer who made this booking.
    /// </summary>
    public Customer? Customer { get; set; }

    /// <summary>
    /// Navigation property to the Payment associated with this booking.
    /// </summary>
    public Payment? Payment { get; set; }

    /// <summary>
    /// Navigation property to the CarBooking details, if this is a car booking.
    /// </summary>
    public CarBooking? CarBooking { get; set; }
    
    /// <summary>
    /// Navigation property to the TripBooking details, if this is a trip booking.
    /// </summary>
    public TripBooking? TripBooking { get; set; }
}

