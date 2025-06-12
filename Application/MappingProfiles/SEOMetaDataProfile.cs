using System;
using Application.DTOs.SEOMetaData;
using AutoMapper;
using Domain.Entities;

namespace Application.MappingProfiles;

/// <summary>
/// AutoMapper profile for mapping between SEO entities and their DTOs.
/// </summary>
public class SEOMetaDataProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SEOMetaDataProfile"/> class.
    /// Configures the mappings for SEO-related DTOs and entities.
    /// </summary>
    public SEOMetaDataProfile()
    {
        CreateMap<SEOMetadata, GetSEOMetaDataDTO>()
        .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
        .ForMember(dest => dest.UrlSlug, opt => opt.MapFrom(src => src.UrlSlug))
        .ForMember(dest => dest.MetaTitle, opt => opt.MapFrom(src => src.MetaTitle))
        .ForMember(dest => dest.MetaDescription, opt => opt.MapFrom(src => src.MetaDescription))
        .ForMember(dest => dest.MetaKeywords, opt => opt.MapFrom(src => src.MetaKeywords))
        .ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.PostId))
        .ReverseMap();

        CreateMap<CreateSEOMetaDataDTO, SEOMetadata>()
        .ForMember(dest => dest.Id, opt => opt.Ignore())
        .ForMember(dest => dest.UrlSlug, opt => opt.MapFrom(src => src.UrlSlug))
        .ForMember(dest => dest.MetaTitle, opt => opt.MapFrom(src => src.MetaTitle))
        .ForMember(dest => dest.MetaDescription, opt => opt.MapFrom(src => src.MetaDescription))
        .ForMember(dest => dest.MetaKeywords, opt => opt.MapFrom(src => src.MetaKeywords))
        .ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.PostId));

        CreateMap<UpdateSEOMetaDataDTO, SEOMetadata>()
        .ForMember(dest => dest.Id, opt => opt.MapFrom(src=>src.Id))
        .ForMember(dest => dest.UrlSlug, opt => opt.MapFrom(src => src.UrlSlug))
        .ForMember(dest => dest.MetaTitle, opt => opt.MapFrom(src => src.MetaTitle))
        .ForMember(dest => dest.MetaDescription, opt => opt.MapFrom(src => src.MetaDescription))
        .ForMember(dest => dest.MetaKeywords, opt => opt.MapFrom(src => src.MetaKeywords))
        .ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.PostId));
    }
}
