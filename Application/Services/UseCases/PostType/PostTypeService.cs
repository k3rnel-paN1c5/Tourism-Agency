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
        private readonly IRepository<Post, int> _postRepository;
        private readonly IMapper _mapper;

        public PostTypeService(IRepository<PostType, int> postTypeRepository, IRepository<Post, int> postRepository,  IMapper mapper)
        {
            _postTypeRepository = postTypeRepository;
            _postRepository = postRepository;
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

        public async Task<PostTypeDto> UpdatePostTypeAsync(UpdatePostTypeDTO postTypeDto)
        {
            if (string.IsNullOrWhiteSpace(postTypeDto.Title))
                throw new Exception("Post type title cannot be empty!");
            if (string.IsNullOrWhiteSpace(postTypeDto.Description))
                throw new Exception("Post type description cannot be empty!");

            var existingPostType = await _postTypeRepository.GetByIdAsync(postTypeDto.Id);
            if (existingPostType == null)
                throw new Exception("Post type not found!");

            _mapper.Map(postTypeDto, existingPostType);

            _postTypeRepository.Update(existingPostType);
            await _postTypeRepository.SaveAsync();

            return _mapper.Map<PostTypeDto>(existingPostType);
        }

        public async Task<bool> DeletePostTypeAsync(int postTypeId)
        {
            var existingPostType = await _postTypeRepository.GetByIdAsync(postTypeId);
            if (existingPostType == null)
                throw new Exception("Post type not found!");

            var defaultPostType = (await _postTypeRepository.GetAllAsync())
                .FirstOrDefault(pt => pt.Title == "Normal Post");
            if (defaultPostType == null)
                throw new Exception("Default post type not found!");

            var postsToUpdate = (await _postRepository.GetAllAsync())
                .Where(p => p.PostTypeId == postTypeId)
                    .ToList();
            foreach (var post in postsToUpdate)
            {
                post.PostTypeId = defaultPostType.Id;
                _postRepository.Update(post);
            } 
            await _postRepository.SaveAsync();

            _postTypeRepository.Delete(existingPostType);
            await _postTypeRepository.SaveAsync();

            return true;
        }

    }
}

