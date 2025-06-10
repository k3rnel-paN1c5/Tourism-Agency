using Application.DTOs.Post;
using Application.DTOs.Tag;

namespace Application.IServices.UseCases;

/// <summary>
/// Defines the contract for Post-related business logic operations.
/// </summary>
public interface IPostService
{
    /// <summary>
    /// Creates a new post asynchronously.
    /// Ensures the specified PostType exists before proceeding.
    /// </summary>
    /// <param name="createPostDto">The DTO containing information for the new post.</param>
    /// <returns>A <see cref="GetPostDTO"/> representing the newly created post.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="createPostDto"/> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the HTTP context is unavailable.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the user ID is missing from the request.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the specified PostType does not exist.</exception>
    public Task<GetPostDTO> CreatePostAsync(CreatePostDTO createPostDto);

    /// <summary>
    /// Retrieves a post by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the post.</param>
    /// <returns>A <see cref="GetPostDTO"/> representing the found post.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the post with the specified ID is not found.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the HTTP context is unavailable.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the user does not have permission to access the post.</exception>
    public Task<GetPostDTO> GetPostByIdAsync(int id);

    /// <summary>
    /// Retrieves all posts asynchronously based on user role.
    /// </summary>
    /// <returns>A collection of <see cref="GetPostDTO"/> representing all accessible posts.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the HTTP context is unavailable.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the user does not have permission to retrieve posts.</exception>
    public Task<IEnumerable<GetPostDTO>> GetAllPostsAsync();

    /// <summary>
    /// Submits a post for review asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the post to submit.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the HTTP context is unavailable.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the user is not an employee or is not the original creator.</exception>
    /// <exception cref="KeyNotFoundException">Thrown if the post with the specified ID is not found.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the post is not in the draft state.</exception>
    public Task SubmitPostAsync(int id);

    /// <summary>
    /// Approves a post for publication asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the post to approve.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the HTTP context is unavailable.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the user is not an administrator.</exception>
    /// <exception cref="KeyNotFoundException">Thrown if the post with the specified ID is not found.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the post is not in the pending state.</exception>
    public Task ApprovePostAsync(int id);

    /// <summary>
    /// Rejects a post and moves it to the unpublished state asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the post to reject.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the HTTP context is unavailable.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the user is not an administrator.</exception>
    /// <exception cref="KeyNotFoundException">Thrown if the post with the specified ID is not found.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the post is not in the pending state.</exception>
    public Task RejectPostAsync(int id);

    /// <summary>
    /// Unpublishes a post and moves it to the unpublished state asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the post to unpublish.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the HTTP context is unavailable.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the user is not an administrator.</exception>
    /// <exception cref="KeyNotFoundException">Thrown if the post with the specified ID is not found.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the post is not in the published state.</exception>
    public Task UnpublishPostAsync(int id);

    /// <summary>
    /// Restores a previously unpublished post and changes its status to Published asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the post to restore.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the HTTP context is unavailable.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the user is not an administrator.</exception>
    /// <exception cref="KeyNotFoundException">Thrown if the post with the specified ID is not found.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the post is not in the unpublished state.</exception>
    public Task RestorePostAsync(int id);

    /// <summary>
    /// Updates an existing post with new data asynchronously.
    /// Only the original creator of the post can update it.
    /// </summary>
    /// <param name="updatePostDto">The DTO containing updated information for the post.</param>
    /// <returns>A <see cref="GetPostDTO"/> representing the updated post.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the HTTP context is unavailable.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the user is not the original creator of the post.</exception>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="updatePostDto"/> is null.</exception>
    /// <exception cref="KeyNotFoundException">Thrown if the post with the specified ID is not found.</exception>
    public Task<GetPostDTO> UpdatePostAsync(UpdatePostDTO updatePostDto);

    /// <summary>
    /// Deletes a post by changing its status to Deleted asynchronously.
    /// Only the original creator of the post can delete it.
    /// </summary>
    /// <param name="id">The unique identifier of the post to delete.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the HTTP context is unavailable.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the user is not the original creator of the post.</exception>
    /// <exception cref="KeyNotFoundException">Thrown if the post with the specified ID is not found.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the post is not in the unpublished state.</exception>
    public Task DeletePostAsync(int id);

    /// <summary>
    /// Assigns a tag to a post asynchronously via PostTag.
    /// </summary>
    /// <param name="postId">The unique identifier of the post.</param>
    /// <param name="tagId">The unique identifier of the tag.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the post or tag with the specified IDs are not found.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the tag is already assigned to the post.</exception>
    /// <exception cref="Exception">Thrown if an error occurs while assigning the tag.</exception>
    public Task AssignTagToPostAsync(int postId, int tagId);

    /// <summary>
    /// Retrieves all tags associated with a specific post asynchronously.
    /// </summary>
    /// <param name="postId">The unique identifier of the post.</param>
    /// <returns>A collection of <see cref="GetTagDTO"/> representing the tags linked to the specified post.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the post with the specified ID is not found.</exception>
    /// <exception cref="Exception">Thrown if an error occurs while retrieving the tags.</exception>
    public Task<IEnumerable<GetTagDTO>> GetTagsByPostIdAsync(int postId);


}


