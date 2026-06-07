using AutoMapper;
using server.DTOs;
using server.Models;
using server.Models.DTO;
using System.Linq;

namespace server.BLL
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // --- 1. מיפוי תורמים (Donor) ---
            // הוסר המיפוי ל-GiftCount כדי שלא יגרום לשגיאות קומפילציה
            CreateMap<Donor, DonorDTO>()
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Gifts, opt => opt.Ignore());

            // --- 2. מיפוי מתנות (Gift) ---
            CreateMap<Gift, GiftDto>()
                // מחבר שם פרטי ומשפחה עבור תצוגת שם התורם במתנה
                .ForMember(dest => dest.DonorName, opt => opt.MapFrom(src =>
                    src.Donor != null ? $"{src.Donor.FirstName} {src.Donor.FamilyName}" : "תורם אנונימי"))
                .ForMember(dest => dest.WinnerName, opt => opt.MapFrom(src =>
                    src.WinnerUser != null ? src.WinnerUser.UserName : null))
                .ReverseMap()
                .ForMember(dest => dest.Donor, opt => opt.Ignore())
                .ForMember(dest => dest.WinnerUser, opt => opt.Ignore())
                .ForMember(dest => dest.Purchases, opt => opt.Ignore());

            // --- 3. מיפוי רכישות (Purchase) ---
            CreateMap<Purchase, PurchaseDTO>()
                .ForMember(dest => dest.GiftName, opt => opt.MapFrom(src => src.Gift != null ? src.Gift.NameGift : ""))
                .ForMember(dest => dest.GiftPrice, opt => opt.MapFrom(src => src.Gift != null ? src.Gift.PriceTicket : 0))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : ""))
                .ReverseMap()
                .ForMember(dest => dest.Gift, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());

            // --- 4. מיפוי בקשת רכישה (PurchaseRequest) ---
            CreateMap<PurchaseRequestDTO, Purchase>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PurchaseDate, opt => opt.Ignore())
                .ForMember(dest => dest.PurchaseStatus, opt => opt.Ignore());
        }
    }
}