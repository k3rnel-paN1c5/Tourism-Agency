using System;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Application.DTOs.Payment;

namespace Application.MappingProfiles
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            // Map from Payment -> ReturnPaymentDTO (Get DTO)
            CreateMap<Payment, ReturnPaymentDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.BookingId, opt => opt.MapFrom(src => src.BookingId))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.AmountDue, opt => opt.MapFrom(src => src.AmountDue))
                .ForMember(dest => dest.AmountPaid, opt => opt.MapFrom(src => src.AmountPaid))
                .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => src.PaymentDate ?? DateTime.MinValue))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
                // .ForMember(dest => dest.Transactions, opt => opt.MapFrom(src => src.PaymentTransactions))
                .ReverseMap();

            // Map from CreatePaymentDTO -> Payment
            CreateMap<CreatePaymentDTO, Payment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BookingId, opt => opt.MapFrom(src => src.BookingId))
                .ForMember(dest => dest.AmountDue, opt => opt.MapFrom(src => src.AmountDue))
                .ForMember(dest => dest.AmountPaid, opt => opt.MapFrom(_ => 0))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => PaymentStatus.Pending))
                .ForMember(dest => dest.PaymentDate, opt => opt.Ignore())
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => string.Empty));

            // Map from UpdatePaymentStatusDTO -> Payment
            CreateMap<UpdatePaymentStatusDTO, Payment>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PaymentId))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.NewStatus))
                // Ignore all other properties
                .ForMember(dest => dest.BookingId, opt => opt.Ignore())
                .ForMember(dest => dest.AmountDue, opt => opt.Ignore())
                .ForMember(dest => dest.AmountPaid, opt => opt.Ignore())
                .ForMember(dest => dest.PaymentDate, opt => opt.Ignore())
                .ForMember(dest => dest.Notes, opt => opt.Ignore());

            // Map from ProcessRefundDTO -> Payment (for refund operations)
            CreateMap<ProcessRefundDTO, Payment>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PaymentId));

            // Map from Payment -> PaymentDetailsDTO (detailed view)
            CreateMap<Payment, PaymentDetailsDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.BookingId, opt => opt.MapFrom(src => src.BookingId))
                .ForMember(dest => dest.AmountDue, opt => opt.MapFrom(src => src.AmountDue))
                .ForMember(dest => dest.AmountPaid, opt => opt.MapFrom(src => src.AmountPaid))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => src.PaymentDate));
        }
    }
}