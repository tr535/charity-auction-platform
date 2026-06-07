using System.ComponentModel.DataAnnotations;

namespace server.DTOs
{
    public class GiftDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "שם המתנה הוא שדה חובה")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "שם המתנה חייב להיות בין 2 ל-100 תווים")]
        public string NameGift { get; set; }

        [Required(ErrorMessage = "חובה לבחור קטגוריה למתנה")]
        [StringLength(50, ErrorMessage = "שם הקטגוריה ארוך מדי")]
        public string Category { get; set; }

        [Required(ErrorMessage = "חובה להזין מחיר לכרטיס")]
        [Range(1, 10000, ErrorMessage = "מחיר כרטיס חייב להיות בין 1 ל-10,000")]
        public decimal PriceTicket { get; set; }

        [Required(ErrorMessage = "חובה לשייך תורם למתנה")]
        public int DonorId { get; set; }

        // שדה להצגה בלבד - ממולא על ידי ה-Service/AutoMapper
        public string? DonorName { get; set; }

        // מספר הרוכשים - מחושב ב-Service
        public int PurchasersCount { get; set; }

        // שם הזוכה - אם קיים, סימן שהמתנה כבר הוגרלה
        public string? WinnerName { get; set; }

        // מאפיין מחושב (Read-only) לשימוש ב-UI
        public bool IsRaffled => !string.IsNullOrEmpty(WinnerName);
    }
}