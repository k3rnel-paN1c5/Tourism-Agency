//namespace Tourism-Agency;
using Application.DTOs.Post;
namespace Application.IServices.UseCases
{
    public interface IPostService
    {
        Task<GetPostDTO> CreatePostAsync(CreatePostDTO dto);//  Create Post
        Task<GetPostDTO> GetPostByIdAsync(int id);
        Task<IEnumerable<GetPostDTO>> GetAllPostsAsync();
        Task SubmitPostAsync(int id);
        Task ApprovePostAsync(int id);
        Task RejectPostAsync(int id);
        Task UnpublishPostAsync(int id);
        Task RestorePostAsync(int id);
        Task<GetPostDTO> UpdatePostAsync(UpdatePostDTO dto); //  Update Post
        Task DeletePostAsync(int id);

    }
}

