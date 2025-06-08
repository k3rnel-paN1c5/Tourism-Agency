namespace Domain.Enums;

/// <summary>
/// Defines the types of financial transactions.
/// </summary>
public enum TransactionType
{
    /// <summary>
    /// A deposit/down payment transaction (partial payment).
    /// </summary>
    Deposit,
    
    /// <summary>
    /// A full payment transaction (complete payment).
    /// </summary>
    Payment,
    
    /// <summary>
    /// A final payment transaction (remaining balance).
    /// </summary>
    Final,
    
    /// <summary>
    /// A refund transaction (money returned to customer).
    /// </summary>
    Refund
}
