using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Customer;

/// <summary>
/// Represents a Data Transfer Object (DTO) for customer registration.
/// This DTO is used when a new customer registers for an account.
/// </summary>
public class CustomerRegisterDTO
{
    /// <summary>
    /// Gets or sets the email address of the customer.
    /// This field is required and must be a valid email format.
    /// </summary>
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [Display(Name = "Email")]
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the password for the customer's account.
    /// This field is required, must be a password type, and has a length constraint of 6-100 characters.
    /// </summary>
    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be 6-100 characters")]
    [Display(Name = "Password")]
    public string? Password { get; set; }

    /// <summary>
    /// Gets or sets the confirmation password for the customer's account.
    /// This field is required, must be a password type, and must match the <see cref="Password"/> field.
    /// </summary>
    [Required(ErrorMessage = "Confirm password is required")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords don't match")]
    [Display(Name = "Confirm Password")]
    public string? ConfirmPassword { get; set; }

    /// <summary>
    /// Gets or sets the first name of the customer.
    /// This field is required and has a length constraint of 2-100 characters.
    /// </summary>
    [Required(ErrorMessage = "First name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "First name must be 2-100 characters")]
    [Display(Name = "First Name")]
    public string? FirstName { get; set; }

    /// <summary>
    /// Gets or sets the last name of the customer.
    /// This field is required and has a length constraint of 2-100 characters.
    /// </summary>
    [Required(ErrorMessage = "Last name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Last name must be 2-100 characters")]
    [Display(Name = "Last Name")]
    public string? LastName { get; set; }

    /// <summary>
    /// Gets or sets the phone number of the customer.
    /// This field is required and must be a 12-digit number.
    /// </summary>
    [Required(ErrorMessage = "Phone number is required")]
    [RegularExpression(@"^[0-9]{12}$", ErrorMessage = "Phone number must be 12 digits")]
    [Display(Name = "Phone Number")]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets the WhatsApp number of the customer.
    /// This field is optional and must be a 14-digit number if provided.
    /// </summary>
    [RegularExpression(@"^[0-9]{14}$", ErrorMessage = "WhatsApp number must be 14 digits")]
    [Display(Name = "Whatsapp")]
    public string? Whatsapp { get; set; }

    /// <summary>
    /// Gets or sets the country of residence for the customer.
    /// This field is required and its name cannot exceed 100 characters.
    /// </summary>
    [Required(ErrorMessage = "Country is required")]
    [StringLength(100, ErrorMessage = "Country name cannot exceed 100 characters")]
    [Display(Name = "Country")]
    public string? Country { get; set; }

}
