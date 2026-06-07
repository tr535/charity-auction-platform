using server.DTOs;
using server.Models;
using System.ComponentModel.DataAnnotations;

namespace server.Models.DTO
{
    public class PurchaseDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "חובה לציין את מזהה המשתמש הרוכש")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "חובה לבחור מתנה לרכישה")]
        public int GiftId { get; set; }

        [Range(0.1, 10000, ErrorMessage = "סכום הרכישה חייב להיות חיובי")]
        public decimal TotalPrice { get; set; }

        // --- השורה שחייבת להתווסף ---
        // בלי זה, Angular לא יצליח לקרוא את item.gift.nameGift
        public GiftDto? Gift { get; set; }

        // שדות אלו יכולים להישאר לגיבוי, אבל ה-Gift למעלה הוא החשוב
        public string? GiftName { get; set; }
        public decimal? GiftPrice { get; set; }
        public string? UserName { get; set; }

        public DateTime PurchaseDate { get; set; } = DateTime.Now;

        public Purchase.Status PurchaseStatus { get; set; }
    }
}