using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

/// <summary>
/// Represents a Transaction Method available (e.g., Credit Card, PayPal).
/// </summary>
public partial class TransactionMethod
{

    /// <summary>
    /// Unique identifier for the Transaction Method.
    /// </summary>
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// Name of the Transaction Method.
    /// </summary>
    [Required]
    [Column("method", TypeName = "nvarchar(50)")]
    public string? Method { get; set; }

    /// <summary>
    /// Icon representing the Transaction Method.
    /// </summary>
    [Column("icon", TypeName = "nvarchar(50)")]
    public string? Icon { get; set; }

    /// <summary>
    /// Indicates whether the Transaction Method is currently active and available for use.
    /// </summary>
    [Column("is_active")]
    public bool IsActive { get; set; } = true;


    // Navigation Properties
    
    /// <summary>
    /// Collection of payment transactions that used this method.
    /// </summary>
    public virtual ICollection<PaymentTransaction>? PaymentTransactions { get; set; }
}