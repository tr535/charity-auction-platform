using AutoMapper;
using server.BLL.Interfaces;
using server.DAL;
using server.Models;
using server.Models.DTO;
using Microsoft.Extensions.Logging; // הוספנו את הלוגר

namespace server.BLL.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseDal _purchaseDal;
        private readonly IGiftDal _giftDal;
        private readonly IMapper _mapper;
        private readonly ILogger<PurchaseService> _logger; // הגדרת הלוגר

        public PurchaseService(IPurchaseDal purchaseDal, IGiftDal giftDal, IMapper mapper, ILogger<PurchaseService> logger)
        {
            _purchaseDal = purchaseDal;
            _giftDal = giftDal;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponseDTO<decimal>> GetTotalRevenueAsync()
        {
            _logger.LogInformation("מחשב סך הכנסות כולל של המכירה הסינית");
            var purchases = await _purchaseDal.GetAllPurchasesAsync();

            var total = purchases
                .Where(p => p.PurchaseStatus == Purchase.Status.Confirmed)
                .Sum(p => p.Gift?.PriceTicket ?? 0);

            _logger.LogInformation("סך ההכנסות המאושרות שחושב: {Total}₪", total);
            return BaseResponseDTO<decimal>.Ok(total, "סך ההכנסות חושב בהצלחה");
        }

        public async Task<BaseResponseDTO<PurchaseDTO>> AddPurchaseAsync(PurchaseRequestDTO purchaseRequest)
        {
            _logger.LogInformation("ניסיון הוספת כרטיס לסל: מתנה {GiftId}, משתמש {UserId}",
                purchaseRequest.GiftId, purchaseRequest.UserId);

            var gift = await _giftDal.GetGiftByIdAsync(purchaseRequest.GiftId);
            if (gift == null)
            {
                _logger.LogWarning("הוספה לסל נכשלה: מתנה {GiftId} לא קיימת", purchaseRequest.GiftId);
                return BaseResponseDTO<PurchaseDTO>.Failure("מתנה לא נמצאה.");
            }

            if (gift.WinnerUserId.HasValue)
            {
                _logger.LogWarning("ניסיון רכישה למתנה {GiftId} שכבר הוגרלה", purchaseRequest.GiftId);
                return BaseResponseDTO<PurchaseDTO>.Failure("המתנה כבר הוגרלה.");
            }

            var purchase = _mapper.Map<Purchase>(purchaseRequest);
            purchase.PurchaseStatus = Purchase.Status.Draft;

            await _purchaseDal.AddPurchaseAsync(purchase);
            await _purchaseDal.SaveChangesAsync();

            _logger.LogInformation("כרטיס נוסף בהצלחה לסל כטיוטה. מזהה רכישה: {PurchaseId}", purchase.Id);

            var createdPurchase = await _purchaseDal.GetPurchaseByIdAsync(purchase.Id);
            return BaseResponseDTO<PurchaseDTO>.Ok(_mapper.Map<PurchaseDTO>(createdPurchase), "הכרטיס נוסף לסל.");
        }

        public async Task<BaseResponseDTO<bool>> ConfirmCartAsync(int userId)
        {
            _logger.LogInformation("מתחיל תהליך אישור סל קניות עבור משתמש {UserId}", userId);

            var purchases = await _purchaseDal.GetPurchasesByUserIdAsync(userId);
            var draftItems = purchases.Where(p => p.PurchaseStatus == Purchase.Status.Draft).ToList();

            if (!draftItems.Any())
            {
                _logger.LogWarning("ניסיון אישור סל נכשל: למשתמש {UserId} אין פריטים בטיוטה", userId);
                return BaseResponseDTO<bool>.Failure("הסל ריק או שכבר אושר.");
            }

            _logger.LogInformation("מאשר {Count} פריטים בסל עבור משתמש {UserId}", draftItems.Count, userId);

            foreach (var item in draftItems)
            {
                item.PurchaseStatus = Purchase.Status.Confirmed;
                await _purchaseDal.UpdatePurchaseAsync(item);
            }

            await _purchaseDal.SaveChangesAsync();
            _logger.LogInformation("סל הקניות של משתמש {UserId} אושר בהצלחה", userId);

            return BaseResponseDTO<bool>.Ok(true, "הרכישה אושרה בהצלחה!");
        }

        public async Task<BaseResponseDTO<bool>> RemovePurchaseAsync(int id)
        {
            _logger.LogInformation("ניסיון הסרת פריט מהסל: רכישה {Id}", id);

            var purchase = await _purchaseDal.GetPurchaseByIdAsync(id);
            if (purchase == null) return BaseResponseDTO<bool>.Failure("רכישה לא נמצאה.");

            if (purchase.PurchaseStatus == Purchase.Status.Confirmed)
            {
                _logger.LogError("חסימת מחיקה: משתמש ניסה למחוק רכישה מאושרת {Id}!", id);
                return BaseResponseDTO<bool>.Failure("לא ניתן למחוק רכישה שכבר אושרה.");
            }

            await _purchaseDal.RemovePurchaseAsync(id);
            await _purchaseDal.SaveChangesAsync();

            _logger.LogInformation("פריט {Id} הוסר מהסל בהצלחה", id);
            return BaseResponseDTO<bool>.Ok(true, "הרכישה הוסרה מהסל.");
        }

        // פונקציות שליפה פשוטות
        public async Task<BaseResponseDTO<IEnumerable<PurchaseDTO>>> GetUserCartAsync(int userId)
        {
            var purchases = await _purchaseDal.GetPurchasesByUserIdAsync(userId);
            return BaseResponseDTO<IEnumerable<PurchaseDTO>>.Ok(_mapper.Map<IEnumerable<PurchaseDTO>>(purchases));
        }

        public async Task<BaseResponseDTO<IEnumerable<PurchaseDTO>>> GetAllPurchasesAsync()
        {
            var purchases = await _purchaseDal.GetAllPurchasesAsync();
            return BaseResponseDTO<IEnumerable<PurchaseDTO>>.Ok(_mapper.Map<IEnumerable<PurchaseDTO>>(purchases));
        }

        public async Task<BaseResponseDTO<IEnumerable<PurchaseDTO>>> GetPurchasesReportAsync(string? sortBy)
        {
            _logger.LogInformation("מפיק דו''ח רכישות מאושרות. מיון לפי: {SortBy}", sortBy ?? "ברירת מחדל");
            var purchases = await _purchaseDal.GetAllPurchasesAsync();
            var query = purchases.Where(p => p.PurchaseStatus == Purchase.Status.Confirmed).AsEnumerable();

            query = sortBy?.ToLower() switch
            {
                "expensive" => query.OrderByDescending(p => p.Gift?.PriceTicket),
                "popular" => query.GroupBy(p => p.GiftId).OrderByDescending(g => g.Count()).SelectMany(g => g),
                _ => query.OrderByDescending(p => p.Id)
            };

            return BaseResponseDTO<IEnumerable<PurchaseDTO>>.Ok(_mapper.Map<IEnumerable<PurchaseDTO>>(query));
        }
    }
}