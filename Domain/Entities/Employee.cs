using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

/// <summary>
/// Represents an employee of the tourism agency.
/// </summary>
public partial class Employee
{
    /// <summary>
    /// Unique user ID of the employee, which also serves as the primary key.
    /// </summary>
    [Key, Column("id", TypeName = "nvarchar(450)")]
    public string? UserId { get; set; }

    /// <summary>
    /// Hire date of the employee.
    /// </summary>
    [Required, Column("hireDate")]
    public DateTime HireDate { get; set; }


    // Navigation Properties

    /// <summary>
    /// Collection of bookings handled by this employee.
    /// </summary>
    public ICollection<Booking>? Bookings { get; set; }

    /// <summary>
    /// Collection of posts created by this employee.
    /// </summary>
    public ICollection<Post>? Posts { get; set; }

}