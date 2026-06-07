
using server.Models; // מייבא את המודלים (כמו User) כדי שנוכל להשתמש בהם כאן
using System.Threading.Tasks; // מאפשר שימוש ב-Task עבור פעולות אסינכרוניות שלא תוקעות את השרת

namespace server.DAL
{
    // הגדרת הממשק - הוא משמש כ"חוזה" שמגדיר אילו פעולות חייבות להיות בשכבת הנתונים
    public interface IUserDal
    {
        // פונקציה לקבלת משתמש לפי אימייל - מחזירה אובייקט User או null אם לא נמצא
        // שימוש: בבדיקת לוגין או כדי למנוע הרשמה כפולה עם אותו אימייל
        Task<User> GetUserByEmailAsync(string email);

        // פונקציה להוספת משתמש חדש לבסיס הנתונים
        // מקבלת אובייקט User מלא ומחזירה אותו לאחר שנשמר (כולל ה-ID שנוצר ב-SQL)
        Task<User> AddUserAsync(User user);

        // פונקציה לשליפת משתמש ספציפי לפי המספר המזהה הייחודי שלו (Primary Key)
        Task<User> GetUserByIdAsync(int id);

        // פונקציה לעדכון נתונים של משתמש קיים (למשל שינוי סיסמה או שינוי סטטוס IsActive)
        // מחזירה true אם העדכון הצליח ו-false אם נכשל
        Task<bool> UpdateUserAsync(User user);

        // פונקציה שבודקת האם משתמש קיים במערכת לפי אימייל - מחזירה אמת או שקר
        Task<bool> IsUserExistsAsync(string email);
    }
}