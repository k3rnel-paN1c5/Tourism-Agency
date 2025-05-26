using AutoMapper;
using Domain.Entities;
using Application.DTOs.Tag;

namespace Application.MappingProfiles
{
    public class TagProfile : Profile
    {
        public TagProfile()
        {
            CreateMap<Tag, CreateTagDTO>().ReverseMap();
            CreateMap<Tag, TagDto>().ReverseMap();
            CreateMap<Tag, UpdateTagDTO>().ReverseMap();
        }
    }
}

