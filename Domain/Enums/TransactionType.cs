namespace Domain.Enums;

/// <summary>
/// Defines the types of financial transactions.
/// </summary>
public enum TransactionType
{
    /// <summary>
    /// A deposit transaction.
    /// </summary>
    Deposit,
    /// <summary>
    /// A final payment transaction.
    /// </summary>
    Final,
    /// <summary>
    /// A refund transaction.
    /// </summary>
    Refund
}