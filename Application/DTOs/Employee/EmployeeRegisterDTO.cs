using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Employee;
public class EmployeeRegisterDTO
{
    public enum TRoles {
        BookingSupervisor,
        TripSupervisor,
        Manager
    }
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string? Email { get; set; } 

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be 6-100 characters")]
    public string? Password { get; set; }

    [Required(ErrorMessage = "Confirm password is required")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords don't match")]
    public string? ConfirmPassword { get; set; }
    [Required(ErrorMessage = "Role is required")]
    public TRoles EmpRole { get; set; }

}
