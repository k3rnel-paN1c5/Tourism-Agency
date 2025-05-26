using Application.DTOs.PostType;

namespace Application.IServices.UseCases
{
    public interface IPostTypeService
    {
        Task<PostTypeDto> CreatePostTypeAsync(CreatePostTypeDTO postTypeDto);
        Task<PostTypeDto> UpdatePostTypeAsync(UpdatePostTypeDTO postTypeDto);
        Task<bool> DeletePostTypeAsync(int postTypeId);
    }
}


