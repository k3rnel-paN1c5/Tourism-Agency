//namespace Tourism-Agency;
using Application.DTOs.Post;
namespace Application.IServices.UseCases
{
    public interface IPostService
    {
 Task<GetPostDTO> CreatePostAsync(CreatePostDTO dto);
    }
}

