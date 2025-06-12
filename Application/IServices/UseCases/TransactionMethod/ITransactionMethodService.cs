using Application.DTOs.TransactionMethod;

namespace Application.IServices.UseCases
{
    /// <summary>
    /// Interface for Transaction Method-related operations.
    /// </summary>
    public interface ITransactionMethodService
    {
        /// <summary>
        /// Creates a new Transaction Method asynchronously.
        /// </summary>
        /// <param name="TransactionMethodDto">The Transaction Method creation data.</param>
        /// <returns>The created Transaction Method information.</returns>
        Task<ReturnTransactionMethodDTO> CreateTransactionMethodAsync(CreateTransactionMethodDTO TransactionMethodDto);

        /// <summary>
        /// Retrieves a Transaction Method by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the Transaction Method to retrieve.</param>
        /// <returns>The Transaction Method information.</returns>
        Task<ReturnTransactionMethodDTO> GetTransactionMethodByIdAsync(int id);

        /// <summary>
        /// Retrieves all Transaction Methods asynchronously.
        /// </summary>
        /// <returns>A collection of all Transaction Methods.</returns>
        Task<IEnumerable<ReturnTransactionMethodDTO>> GetAllTransactionMethodsAsync();

        /// <summary>
        /// Retrieves all active Transaction Methods asynchronously.
        /// </summary>
        /// <returns>A collection of active Transaction Methods.</returns>
        Task<IEnumerable<ReturnTransactionMethodDTO>> GetActiveTransactionMethodsAsync();

        /// <summary>
        /// Updates an existing Transaction Method asynchronously.
        /// </summary>
        /// <param name="TransactionMethodDto">The Transaction Method update data.</param>
        /// <returns>The updated Transaction Method information.</returns>
        Task<ReturnTransactionMethodDTO> UpdateTransactionMethodAsync(UpdateTransactionMethodDTO TransactionMethodDto);

        /// <summary>
        /// Deletes a Transaction Method asynchronously.
        /// </summary>
        /// <param name="id">The ID of the Transaction Method to delete.</param>
        /// <returns>True if the deletion was successful, otherwise false.</returns>
        Task<bool> DeleteTransactionMethodAsync(int id);

        /// <summary>
        /// Toggles the active status of a Transaction Method asynchronously.
        /// </summary>
        /// <param name="id">The ID of the Transaction Method to toggle.</param>
        /// <returns>The updated Transaction Method information.</returns>
        Task<ReturnTransactionMethodDTO> ToggleTransactionMethodStatusAsync(int id);
    }
}
