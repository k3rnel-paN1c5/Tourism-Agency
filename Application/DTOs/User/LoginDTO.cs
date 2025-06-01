using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.User
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) for user login.
    /// This DTO is used to authenticate users by their credentials.
    /// </summary>
    public class LoginDTO
    {
        /// <summary>
        /// Gets or sets the email address of the user attempting to log in.
        /// This field is required and must be a valid email format.
        /// </summary>
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the password of the user attempting to log in.
        /// This field is required and is treated as a password type.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user wishes to be remembered (e.g., for persistent login sessions).
        /// </summary>
        public bool RememberMe { get; set; }
    }
}