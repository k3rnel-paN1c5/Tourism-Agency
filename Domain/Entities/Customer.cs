using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

/// <summary>
/// Represents a customer of the tourism agency.
/// </summary>
public partial class Customer
{
    /// <summary>
    /// Unique user ID of the customer, which also serves as the primary key.
    /// </summary>
    [Key, Column("id", TypeName = "nvarchar(450)")]
    public string? UserId { get; set; }

    /// <summary>
    /// First name of the customer.
    /// </summary>
    [Required]
    [Column("firstName")]
    public string? FirstName { get; set; }

    /// <summary>
    /// Last name of the customer.
    /// </summary>
    [Required, Column("lastName")]
    public string? LastName { get; set; }

    /// <summary>
    /// Phone number of the customer.
    /// </summary>
    [Required, Column("phoneNumber", TypeName = "char(12)")]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// WhatsApp number of the customer.
    /// </summary>
    [Column("whatsapp", TypeName = "char(14)")]
    public string? Whatsapp { get; set; }

    /// <summary>
    /// Country of residence for the customer.
    /// </summary>
    [Column("Country")]
    public string? Country { get; set; }


    //  Navigation Properties
    
    /// <summary>
    /// Collection of bookings made by this customer.
    /// </summary>
    public ICollection<Booking>? Bookings { get; set; }
}

