using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

/// <summary>
/// Represents a payment method available (e.g., Credit Card, PayPal).
/// </summary>
public partial class PaymentMethod
{

    /// <summary>
    /// Unique identifier for the payment method.
    /// </summary>
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// Name of the payment method.
    /// </summary>
    [Required]
    [Column("method", TypeName = "nvarchar(50)")]
    public string? Method { get; set; }

    /// <summary>
    /// Icon representing the payment method.
    /// </summary>
    [Column("icon", TypeName = "nvarchar(50)")]
    public string? Icon { get; set; }


    // Navigation Properties
    
    /// <summary>
    /// Collection of payment transactions that used this method.
    /// </summary>
    public virtual ICollection<PaymentTransaction>? PaymentTransactions { get; set; }
}

