using EF_core.DAL;
using Microsoft.EntityFrameworkCore;
using server.DAL;
using server.Models;
using System;

public class UserDal : IUserDal // מחלקה זו חייבת לממש את כל מה שהגדרנו ב-Interface
{
    private readonly ChineseSaleContext _context; // משתנה שיחזיק את הקשר לבסיס הנתונים

    // הזרקה של ה-DbContext דרך הקונסטרקטור
    public UserDal(ChineseSaleContext context)
    {
        _context = context;
    }

    // חיפוש משתמש לפי אימייל
    public async Task<User> GetUserByEmailAsync(string email)
    {
        // מחפש בטבלת Users את השורה שבה האימייל זהה לאימייל שהתקבל
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    // הוספת משתמש חדש
    public async Task<User> AddUserAsync(User user)
    {
        await _context.Users.AddAsync(user); // הוספה זמנית לזיכרון של ה-DbContext
        await _context.SaveChangesAsync();   // שמירה סופית של השינויים בתוך ה-SQL Server
        return user; // מחזיר את המשתמש עם ה-ID האוטומטי שנוצר
    }

    // שליפת משתמש לפי ה-ID שלו
    public async Task<User> GetUserByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    // בדיקה מהירה אם אימייל כבר תפוס
    public async Task<bool> IsUserExistsAsync(string email)
    {
        // מחזיר אמת אם קיימת לפחות שורה אחת עם האימייל הזה
        return await _context.Users.AnyAsync(u => u.Email == email);
    }

    // עדכון פרטי משתמש
    public async Task<bool> UpdateUserAsync(User user)
    {
        _context.Users.Update(user); // מסמן למערכת שהאובייקט השתנה
        var result = await _context.SaveChangesAsync(); // שומר את העדכון
        return result > 0; // אם יותר משורה אחת הושפעה, העדכון הצליח
    }
}
