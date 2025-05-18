using AutoMapper;
using System;
using Domain.Entities;
using Application.DTOs.Category;
namespace Application.MappingProfiles
{
    public class CategoryProfile:Profile 
    {
        public CategoryProfile() {
            CreateMap<Category, GetCategoryDTO>();
            CreateMap<CreateCategoryDTO, Category>();
            CreateMap<UpdateCategoryDTO, Category>();
 
        
        }
    }
}
