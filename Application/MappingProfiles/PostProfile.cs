//namespace Tourism-Agency;
using AutoMapper;
using Domain.Entities;
using Application.DTOs.Post;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;


namespace Application.MappingProfiles
{
    public class PostProfile : Profile
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PostProfile(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            var userId = _httpContextAccessor
                    .HttpContext
                    .User.Claims
                    .FirstOrDefault(c => c.Type == "UserId" ||
                                     c.Type == "sub" ||
                                     c.Type == ClaimTypes.NameIdentifier)?.Value;
            // Mapping CreatePostDTO to Post
            // Mapping CreatePostDTO to Post
            CreateMap<CreatePostDTO, Post>()
            .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => userId)) ;

            // Mapping Post to GetPostDTO
            // Mapping Post to GetPostDTO
            CreateMap<Post, GetPostDTO>();

            // Mapping UpdatePostDTO to Post
            CreateMap<UpdatePostDTO, Post>();


        }
    }
}