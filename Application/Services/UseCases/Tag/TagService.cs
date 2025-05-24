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

            return _mapper.Map<TagDto>(tag); // إرجاع البيانات الكاملة بعد الإدراج
        }
    }
}

