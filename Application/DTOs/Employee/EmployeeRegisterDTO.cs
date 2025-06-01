using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Employee;
/// <summary>
/// Represents a Data Transfer Object (DTO) for employee registration.
/// This DTO is used when a new employee account is created.
/// </summary>
public class EmployeeRegisterDTO
{
    /// <summary>
    /// Defines the possible roles for an employee.
    /// </summary>
    public enum TRoles {
        /// <summary>
        /// Employee role for managing car-related operations.
        /// </summary>
        CarSupervisor,
        /// <summary>
        /// Employee role for managing trip-related operations.
        /// </summary>
        TripSupervisor,
        /// <summary>
        /// Employee role with managerial privileges.
        /// </summary
        Manager
    }

    /// <summary>
    /// Gets or sets the email address of the employee.
    /// This field is required and must be a valid email format.
    /// </summary>
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the password for the employee's account.
    /// This field is required, must be a password type, and has a length constraint of 6-100 characters.
    /// </summary>
    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be 6-100 characters")]
    public string? Password { get; set; }

    /// <summary>
    /// Gets or sets the confirmation password for the employee's account.
    /// This field is required, must be a password type, and must match the <see cref="Password"/> field.
    /// </summary>
    [Required(ErrorMessage = "Confirm password is required")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords don't match")]
    public string? ConfirmPassword { get; set; }

    /// <summary>
    /// Gets or sets the role of the employee.
    /// This field is required.
    /// </summary>
    [Required(ErrorMessage = "Role is required")]
    public TRoles EmpRole { get; set; }

}
