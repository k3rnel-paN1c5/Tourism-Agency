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
        public PostProfile()
        {

            // Mapping CreatePostDTO to Post
            // Mapping CreatePostDTO to Post
            CreateMap<CreatePostDTO, Post>();

            // Mapping Post to GetPostDTO
            // Mapping Post to GetPostDTO
            CreateMap<Post, GetPostDTO>();

            // Mapping UpdatePostDTO to Post
            CreateMap<UpdatePostDTO, Post>();


        }
    }
}