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
        public async Task<TagDto> UpdateTagAsync(UpdateTagDTO tagDto)
        {
            // التحقق من صحة الإدخال
            if (string.IsNullOrWhiteSpace(tagDto.Name))
                throw new Exception("Tag name cannot be empty!");

            // استرجاع العلامة من قاعدة البيانات
            var existingTag = await _tagRepository.GetByIdAsync(tagDto.Id);
            if (existingTag == null)
                throw new Exception("Tag not found!");

            // التحقق من عدم وجود اسم مكرر
            var duplicateTag = (await _tagRepository.GetAllAsync())
                     .FirstOrDefault(t => t.Name == tagDto.Name && t.Id != tagDto.Id);

            if (duplicateTag != null)
                throw new Exception("Tag name already exists!");

            // تحديث القيم باستخدام `Mapper`
            _mapper.Map(tagDto, existingTag);

            // حفظ التحديثات
            _tagRepository.Update(existingTag);
            await _tagRepository.SaveAsync();

            // إعادة البيانات بعد التحديث
            return _mapper.Map<TagDto>(existingTag);
        
        }
            
    }
}

