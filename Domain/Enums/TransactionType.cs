namespace Domain.Enums;

/// <summary>
/// Defines the types of financial transactions.
/// </summary>
public enum TransactionType
{
    /// <summary>
    /// A payment transaction (money received from customer).
    /// </summary>
    Payment,
    
    /// <summary>
    /// A refund transaction (money returned to customer).
    /// </summary>
    Refund
}
