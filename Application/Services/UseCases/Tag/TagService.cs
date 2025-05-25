using Application.DTOs.Tag;
using Application.IServices.UseCases;
using AutoMapper;
using Domain.Entities;
using Domain.IRepositories;

namespace Application.Services.UseCases
{
    public class TagService : ITagService
    {
        private readonly IRepository<Tag, int> _tagRepository;
        private readonly IMapper _mapper;

        public TagService(IRepository<Tag, int> tagRepository, IMapper mapper)
        {
            _tagRepository = tagRepository;
            _mapper = mapper;
        }

        public async Task<TagDto> CreateTagAsync(CreateTagDTO tagDto)
        {
            if (string.IsNullOrWhiteSpace(tagDto.Name))
                throw new Exception("Tag name cannot be empty!");

            var tag = _mapper.Map<Tag>(tagDto);
            await _tagRepository.AddAsync(tag);
            await _tagRepository.SaveAsync();

            return _mapper.Map<TagDto>(tag); 
        }
        public async Task<TagDto> UpdateTagAsync(UpdateTagDTO tagDto)
        {
            if (string.IsNullOrWhiteSpace(tagDto.Name))
                throw new Exception("Tag name cannot be empty!");

            var existingTag = await _tagRepository.GetByIdAsync(tagDto.Id);
            if (existingTag == null)
                throw new Exception("Tag not found!");

            var duplicateTag = (await _tagRepository.GetAllAsync())
                     .FirstOrDefault(t => t.Name == tagDto.Name && t.Id != tagDto.Id);

            if (duplicateTag != null)
                throw new Exception("Tag name already exists!");

            _mapper.Map(tagDto, existingTag);

            _tagRepository.Update(existingTag);
            await _tagRepository.SaveAsync();

            return _mapper.Map<TagDto>(existingTag);

        }
        public async Task<bool> DeleteTagAsync(int tagId)
        {
            var existingTag = await _tagRepository.GetByIdAsync(tagId);
            if (existingTag == null)
             throw new Exception("Tag not found!");

            _tagRepository.Delete(existingTag);
            await _tagRepository.SaveAsync();

            return true; 
        }

            
    }
}

