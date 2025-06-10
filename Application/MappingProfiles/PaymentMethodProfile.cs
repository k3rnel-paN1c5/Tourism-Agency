using AutoMapper;
using Domain.Entities;
using Application.DTOs.PaymentMethod;

namespace Application.MappingProfiles
{
    public class PaymentMethodProfile : Profile
    {
        public PaymentMethodProfile()
        {
            // Map from CreatePaymentMethodDTO -> PaymentMethod
            CreateMap<CreatePaymentMethodDTO, PaymentMethod>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Method, opt => opt.MapFrom(src => src.Method.Trim()))
                .ForMember(dest => dest.Icon, opt => opt.MapFrom(src => src.Icon != null ? src.Icon.Trim() : string.Empty))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.PaymentTransactions, opt => opt.Ignore());

            // Map from PaymentMethod -> ReturnPaymentMethodDTO
            CreateMap<PaymentMethod, ReturnPaymentMethodDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Method, opt => opt.MapFrom(src => src.Method))
                .ForMember(dest => dest.Icon, opt => opt.MapFrom(src => src.Icon))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

            // Map from UpdatePaymentMethodDTO -> PaymentMethod (for partial updates)
            CreateMap<UpdatePaymentMethodDTO, PaymentMethod>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Method, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.Method) ? src.Method.Trim() : src.Method))
                .ForMember(dest => dest.Icon, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.Icon) ? src.Icon.Trim() : src.Icon))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.PaymentTransactions, opt => opt.Ignore());
        }
    }
}