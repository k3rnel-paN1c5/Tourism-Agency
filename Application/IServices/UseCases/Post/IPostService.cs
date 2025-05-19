//namespace Tourism-Agency;
using Application.DTOs.Post;
namespace Application.IServices.UseCases.Post
{
    public interface IPostService
    {
        Task<GetPostDTO> CreatePostAsync(CreatePostDTO dto);//  Create Post
        Task<GetPostDTO> UpdatePostAsync(UpdatePostDTO dto); //  Update Post
        Task<bool> DeletePostAsync(int id);

    }
}

