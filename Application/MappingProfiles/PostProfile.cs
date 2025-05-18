//namespace Tourism-Agency;
using AutoMapper;
using Domain.Entities;
using Application.DTOs.Post;
namespace Application.MappingProfiles
{
public class PostProfile:Profile
{
        public PostProfile()
        {
            // Modify CreatePostDTO to Post
            CreateMap<CreatePostDTO, Post>();

            // Modify Post to GetPostDTO
            CreateMap<Post, GetPostDTO>();
        }  
}
}

