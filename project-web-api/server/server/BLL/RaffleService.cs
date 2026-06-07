using server.BLL.Interfaces;
using server.DAL;
using server.Models;
using server.Models.DTO;
using Microsoft.Extensions.Logging;

namespace server.BLL.Services
{
    public class RaffleService : IRaffleService
    {
        private readonly IPurchaseDal _purchaseDal;
        private readonly IGiftDal _giftDal;
        private readonly ILogger<RaffleService> _logger;

        public RaffleService(IPurchaseDal purchaseDal, IGiftDal giftDal, ILogger<RaffleService> logger)
        {
            _purchaseDal = purchaseDal;
            _giftDal = giftDal;
            _logger = logger;
        }

        public async Task<BaseResponseDTO<RaffleResultDto>> ExecuteDrawAsync(int giftId)
        {
            _logger.LogInformation("מפעיל הגרלה למתנה: {GiftId}", giftId);

            // 1. שליפת כל הרכישות - חשוב לוודא שב-DAL ה-GetAllPurchases כולל Include ל-User
            var allPurchases = await _purchaseDal.GetAllPurchasesAsync();
            var validPurchases = allPurchases
                .Where(p => p.GiftId == giftId && p.PurchaseStatus == Purchase.Status.Confirmed)
                .ToList();

            if (!validPurchases.Any())
                return BaseResponseDTO<RaffleResultDto>.Failure("לא ניתן לבצע הגרלה - אין כרטיסים מאושרים.");

            // 2. בחירת זוכה
            var winningPurchase = validPurchases[new Random().Next(validPurchases.Count)];

            // 3. שליפת המתנה לעדכון
            var gift = await _giftDal.GetGiftByIdAsync(giftId);
            if (gift == null) return BaseResponseDTO<RaffleResultDto>.Failure("מתנה לא נמצאה.");
            if (gift.WinnerUserId.HasValue) return BaseResponseDTO<RaffleResultDto>.Failure("כבר בוצעה הגרלה למתנה זו.");

            // 4. עדכון הזוכה במתנה
            gift.WinnerUserId = winningPurchase.UserId;
            await _giftDal.UpdateGiftAsync(gift);

            // 5. עדכון סטטוס הרכישה הספציפית ל"זוכה"
            var purchaseToUpdate = await _purchaseDal.GetPurchaseByIdAsync(winningPurchase.Id);
            if (purchaseToUpdate != null)
            {
                purchaseToUpdate.PurchaseStatus = Purchase.Status.Winner;
                await _purchaseDal.UpdatePurchaseAsync(purchaseToUpdate);
            }

            // 6. שמירת השינויים ב-DB
            await _giftDal.SaveChangesAsync();

            // 7. החזרת הנתונים כולל אימייל עבור ה-Frontend (EmailJS)
            return BaseResponseDTO<RaffleResultDto>.Ok(new RaffleResultDto
            {
                WinnerName = winningPurchase.User?.UserName ?? "זוכה",
                WinnerEmail = winningPurchase.User?.Email, // קריטי לשליחת מייל
                GiftId = giftId,
                GiftName = gift.NameGift,
                Message = $"הזוכה במתנה '{gift.NameGift}' הוא: {winningPurchase.User?.UserName ?? "מזהה " + winningPurchase.UserId}"
            });
        }

        public async Task<BaseResponseDTO<decimal>> GetTotalIncomeAsync()
        {
            var all = await _purchaseDal.GetAllPurchasesAsync();
            var income = (decimal)all
                .Where(p => p.Gift != null && (p.PurchaseStatus == Purchase.Status.Confirmed || p.PurchaseStatus == Purchase.Status.Winner))
                .Sum(p => p.Gift.PriceTicket);
            return BaseResponseDTO<decimal>.Ok(income);
        }

        public async Task<BaseResponseDTO<List<RaffleResultDto>>> GetWinnersReportAsync()
        {
            var gifts = await _giftDal.GetAllGiftsAsync();
            var winners = gifts
                .Where(g => g.WinnerUserId.HasValue)
                .Select(g => new RaffleResultDto
                {
                    WinnerName = g.WinnerUser?.UserName ?? "זוכה",
                    WinnerEmail = g.WinnerUser?.Email, // הוספה לדו"ח במידת הצורך
                    GiftId = g.Id,
                    GiftName = g.NameGift,
                    Message = $"זכה ב: {g.NameGift}"
                }).ToList();
            return BaseResponseDTO<List<RaffleResultDto>>.Ok(winners);
        }
    }
}