using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Customer;

public class CustomerRegisterDTO
{
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

    [Required(ErrorMessage = "First name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "First name must be 2-100 characters")]
    public string? FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Last name must be 2-100 characters")]
    public string? LastName { get; set; } 

    [Required(ErrorMessage = "Phone number is required")]
    [RegularExpression(@"^[0-9]{12}$", ErrorMessage = "Phone number must be 12 digits")]
    public string? PhoneNumber { get; set; }

    [RegularExpression(@"^[0-9]{14}$", ErrorMessage = "WhatsApp number must be 14 digits")]
    public string? Whatsapp { get; set; }

    [Required(ErrorMessage = "Country is required")]
    [StringLength(100, ErrorMessage = "Country name cannot exceed 100 characters")]
    public string? Country { get; set; }

}
