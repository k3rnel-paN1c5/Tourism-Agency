using Application.DTOs.PostType;
using Application.IServices.UseCases;
using AutoMapper;
using Domain.Entities;
using Domain.IRepositories;

namespace Application.Services.UseCases
{
    public class PostTypeService : IPostTypeService
    {
        private readonly IRepository<PostType, int> _postTypeRepository;
        private readonly IMapper _mapper;

        public PostTypeService(IRepository<PostType, int> postTypeRepository, IMapper mapper)
        {
            _postTypeRepository = postTypeRepository;
            _mapper = mapper;
        }

        public async Task<PostTypeDto> CreatePostTypeAsync(CreatePostTypeDTO postTypeDto)
        {
            if (string.IsNullOrWhiteSpace(postTypeDto.Title))
                throw new Exception("Post type title cannot be empty!");
            if (string.IsNullOrWhiteSpace(postTypeDto.Description))
                throw new Exception("Post type description cannot be empty!");

            var postType = _mapper.Map<PostType>(postTypeDto);
            await _postTypeRepository.AddAsync(postType);
            await _postTypeRepository.SaveAsync();

            return _mapper.Map<PostTypeDto>(postType);
        }
    }
}

