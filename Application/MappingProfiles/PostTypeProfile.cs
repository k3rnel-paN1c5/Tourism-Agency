using AutoMapper;
using Domain.Entities;
using Application.DTOs.PostType;

namespace Application.MappingProfiles
{
    public class PostTypeProfile : Profile
    {
        public PostTypeProfile()
        {
            CreateMap<PostType, CreatePostTypeDTO>().ReverseMap();
            CreateMap<PostType, PostTypeDto>().ReverseMap();
        }
    }
}

