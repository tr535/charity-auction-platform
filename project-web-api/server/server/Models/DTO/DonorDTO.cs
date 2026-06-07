using server.DTOs;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace server.Models.DTO
{
    public class DonorDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "שם פרטי הוא שדה חובה")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "שם פרטי חייב להיות בין 2 ל-50 תווים")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "שם משפחה הוא שדה חובה")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "שם משפחה חייב להיות בין 2 ל-50 תווים")]
        public string FamilyName { get; set; } = string.Empty;

        [Required(ErrorMessage = "כתובת אימייל היא שדה חובה")]
        [EmailAddress(ErrorMessage = "פורמט האימייל אינו תקין")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "מספר טלפון לא תקין")]
        public string? Phone { get; set; }

        // --- תוספות לפי דרישות הפרויקט ---

        // רשימת המתנות שהתורם תרם (חובה לפי הגדרת הפרויקט)
        public List<GiftDto> Gifts { get; set; } = new List<GiftDto>();

        // שדה עזר שסופר את המתנות (יעזור ל-React להציג את המספר בקלות)
        public int GiftCount => Gifts?.Count ?? 0;

        // לצורך תצוגה בלבד ב-Frontend
        public string FullName => $"{FirstName} {FamilyName}";
    }
}