using Application.DTOs.PaymentTransaction;
using AutoMapper;
using Domain.Entities;

namespace Application.MappingProfiles
{
    /// <summary>
    /// AutoMapper profile for PaymentTransaction entity mappings.
    /// </summary>
    public class PaymentTransactionProfile : Profile
    {
        public PaymentTransactionProfile()
        {
            // CreatePaymentTransactionDTO to PaymentTransaction
            CreateMap<CreatePaymentTransactionDTO, PaymentTransaction>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.TransactionDate, opt => opt.Ignore()) // Set in service
                .ForMember(dest => dest.Payment, opt => opt.Ignore())
                .ForMember(dest => dest.TransactionMethod, opt => opt.Ignore());

            // PaymentTransaction to ReturnPaymentTransactionDTO
            CreateMap<PaymentTransaction, ReturnPaymentTransactionDTO>()
                .ForMember(dest => dest.TransactionMethodName, opt => opt.Ignore()); // Set in service

            // PaymentTransaction to PaymentTransactionDetailsDTO
            CreateMap<PaymentTransaction, PaymentTransactionDetailsDTO>()
                .ForMember(dest => dest.TransactionMethodName, opt => opt.Ignore()) // Set in service
                .ForMember(dest => dest.TransactionMethodIcon, opt => opt.Ignore()) // Set in service
                .ForMember(dest => dest.BookingId, opt => opt.Ignore()) // Set in service
                .ForMember(dest => dest.PaymentStatus, opt => opt.Ignore()); // Set in service

            // UpdatePaymentTransactionDTO to PaymentTransaction (for updating)
            CreateMap<UpdatePaymentTransactionDTO, PaymentTransaction>()
                .ForMember(dest => dest.TransactionType, opt => opt.Ignore())
                .ForMember(dest => dest.Amount, opt => opt.Ignore())
                .ForMember(dest => dest.TransactionDate, opt => opt.Ignore())
                .ForMember(dest => dest.PaymentId, opt => opt.Ignore())
                .ForMember(dest => dest.TransactionMethodId, opt => opt.Ignore())
                .ForMember(dest => dest.Payment, opt => opt.Ignore())
                .ForMember(dest => dest.TransactionMethod, opt => opt.Ignore());
        }
    }
}