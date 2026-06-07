using System.ComponentModel.DataAnnotations;
using server.Models; // ודאי ש-UserRole (Enum) מוגדר כאן

namespace server.Models.DTO
{
    public class UserDTO
    {
        [Required(ErrorMessage = "שם משתמש הוא שדה חובה")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "שם משתמש חייב להיות בין 2 ל-30 תווים")]
        [RegularExpression(@"^[a-zA-Z0-0א-ת._\s]+$", ErrorMessage = "שם משתמש יכול להכיל אותיות, מספרים, נקודה או קו תחתון")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "סיסמה היא שדה חובה")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "הסיסמה חייבת להיות בין 6 ל-20 תווים")]
        // ולידציה נוספת לאבטחה: מחייב לפחות אות אחת ומספר אחד
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$", ErrorMessage = "הסיסמה חייבת להכיל לפחות אות אחת ומספר אחד")]
        public string Password { get; set; }

        [Required(ErrorMessage = "כתובת אימייל היא שדה חובה")]
        [EmailAddress(ErrorMessage = "פורמט אימייל אינו תקין")]
        public string Email { get; set; }

        [Required(ErrorMessage = "מספר טלפון הוא שדה חובה")]
        [Phone(ErrorMessage = "מספר טלפון אינו תקין")]
        [RegularExpression(@"^\d{9,10}$", ErrorMessage = "מספר טלפון חייב להכיל 9 או 10 ספרות")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "חובה להגדיר תפקיד למשתמש")]
        public UserRole Role { get; set; } = UserRole.Customer;
    }
}
