using Application.DTOs.Tag;

namespace Application.IServices.UseCases
{
    public interface ITagService
    {
        Task<TagDto> CreateTagAsync(CreateTagDTO tagDto);
    }
}

