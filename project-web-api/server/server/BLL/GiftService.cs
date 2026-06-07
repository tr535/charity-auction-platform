using AutoMapper;
using Microsoft.EntityFrameworkCore;
using server.BLL.Interfaces;
using server.DAL;
using server.Models;
using server.Models.DTO;
using server.DTOs;
using Microsoft.Extensions.Logging; // חייב עבור הלוגר

namespace server.BLL.Services
{
    public class GiftService : IGiftService
    {
        private readonly IGiftDal _giftDal;
        private readonly IDonorDal _donorDal;
        private readonly IMapper _mapper;
        private readonly ILogger<GiftService> _logger; // הגדרת הלוגר

        public GiftService(IGiftDal giftDal, IDonorDal donorDal, IMapper mapper, ILogger<GiftService> logger)
        {
            _giftDal = giftDal;
            _donorDal = donorDal;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponseDTO<IEnumerable<GiftDto>>> GetAllGiftsAsync(
            string? name,
            string? category,
            string? donorName,
            int? minPurchasers,
            string? sortBy)
        {
            _logger.LogInformation("שליפת מתנות עם פילטרים: שם={Name}, קטגוריה={Category}, מיון={SortBy}", name, category, sortBy);

            var giftsQuery = _giftDal.GetGiftsQueryable();

            if (!string.IsNullOrEmpty(name))
                giftsQuery = giftsQuery.Where(g => g.NameGift.Contains(name));

            if (!string.IsNullOrEmpty(category))
                giftsQuery = giftsQuery.Where(g => g.Category == category);

            if (!string.IsNullOrEmpty(donorName))
                giftsQuery = giftsQuery.Where(g => g.Donor != null && g.Donor.FirstName.Contains(donorName));

            if (minPurchasers.HasValue)
                giftsQuery = giftsQuery.Where(g => g.Purchases.Count >= minPurchasers.Value);

            giftsQuery = sortBy?.ToLower() switch
            {
                "price" => giftsQuery.OrderBy(g => g.PriceTicket),
                "pricedesc" => giftsQuery.OrderByDescending(g => g.PriceTicket),
                "category" => giftsQuery.OrderBy(g => g.Category),
                "popular" => giftsQuery.OrderByDescending(g => g.Purchases.Count),
                _ => giftsQuery.OrderBy(g => g.Id)
            };

            var gifts = await giftsQuery.ToListAsync();
            _logger.LogInformation("נמצאו {Count} מתנות התואמות לחיפוש", gifts.Count);

            return BaseResponseDTO<IEnumerable<GiftDto>>.Ok(_mapper.Map<IEnumerable<GiftDto>>(gifts));
        }

        public async Task<BaseResponseDTO<GiftDto>> GetGiftByIdAsync(int id)
        {
            var gift = await _giftDal.GetGiftByIdAsync(id);
            if (gift == null)
            {
                _logger.LogWarning("ניסיון שליפת מתנה נכשל: מזהה {Id} לא קיים", id);
                return BaseResponseDTO<GiftDto>.Failure("מתנה לא נמצאה.");
            }
            return BaseResponseDTO<GiftDto>.Ok(_mapper.Map<GiftDto>(gift));
        }

        public async Task<BaseResponseDTO<GiftDto>> AddGiftAsync(GiftDto giftDto)
        {
            _logger.LogInformation("הוספת מתנה חדשה: {Name}, מחיר כרטיס: {Price}", giftDto.NameGift, giftDto.PriceTicket);

            var gift = _mapper.Map<Gift>(giftDto);
            await _giftDal.AddGiftAsync(gift);
            await _giftDal.SaveChangesAsync();

            // טעינת ישות התורם מה-DB לצורך ה-Response
            gift.Donor = await _donorDal.GetDonorByIdAsync(gift.DonorId);

            _logger.LogInformation("המתנה {Id} נוספה בהצלחה ושוייכה לתורם {DonorId}", gift.Id, gift.DonorId);

            return BaseResponseDTO<GiftDto>.Ok(_mapper.Map<GiftDto>(gift), "המתנה נוספה בהצלחה.");
        }

        public async Task<BaseResponseDTO<bool>> UpdateGiftAsync(int id, GiftDto giftDto)
        {
            var existing = await _giftDal.GetGiftByIdAsync(id);
            if (existing == null) return BaseResponseDTO<bool>.Failure("מתנה לא נמצאה.");

            if (existing.WinnerUserId.HasValue)
            {
                _logger.LogError("חסימת עדכון: ניסיון לעדכן את מתנה {Id} לאחר שכבר בוצעה הגרלה!", id);
                return BaseResponseDTO<bool>.Failure("לא ניתן לעדכן מתנה לאחר שההגרלה התקיימה.");
            }

            _mapper.Map(giftDto, existing);
            await _giftDal.UpdateGiftAsync(existing);
            await _giftDal.SaveChangesAsync();

            _logger.LogInformation("מתנה {Id} עודכנה בהצלחה", id);
            return BaseResponseDTO<bool>.Ok(true, "המתנה עודכנה בהצלחה.");
        }

        public async Task<BaseResponseDTO<bool>> RemoveGiftAsync(int id)
        {
            _logger.LogWarning("ניסיון הסרת מתנה מזהה {Id}", id);

            var gift = await _giftDal.GetGiftByIdAsync(id);
            if (gift == null) return BaseResponseDTO<bool>.Failure("מתנה לא נמצאה.");

            // בדיקה אם יש רכישות שהן לא טיוטה
            if (gift.Purchases != null && gift.Purchases.Any(p => p.PurchaseStatus != Purchase.Status.Draft))
            {
                _logger.LogError("חסימת מחיקה: למתנה {Id} יש רכישות סופיות במערכת", id);
                return BaseResponseDTO<bool>.Failure("לא ניתן למחוק מתנה שכבר נרכשו עבורה כרטיסים.");
            }

            await _giftDal.RemoveGiftAsync(id);
            await _giftDal.SaveChangesAsync();

            _logger.LogInformation("מתנה {Id} הוסרה מהמערכת בהצלחה", id);
            return BaseResponseDTO<bool>.Ok(true, "המתנה הוסרה בהצלחה.");
        }
    }
}