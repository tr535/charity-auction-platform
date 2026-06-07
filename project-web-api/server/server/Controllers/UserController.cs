using Microsoft.AspNetCore.Mvc;
using server.BLL.Interfaces; // שימוש בממשק כפי שמוגדר בשכבת ה-BLL
using server.Models.DTO;
using System.Threading.Tasks;

namespace server.Controllers
{
    [ApiController] // מציין שמחלקה זו היא API ומפעיל בדיקות תקינות אוטומטיות
    [Route("api/[controller]")] // הכתובת: api/Auth
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        // הזרקת התלות (Dependency Injection) של שירות המשתמשים
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        // ==========================================
        // Register - רישום משתמש חדש
        // ==========================================
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            // פנייה לשירות לביצוע הרישום
            var result = await _userService.RegisterAsync(dto);

            // אם הרישום נכשל (למשל: מייל כבר קיים)
            if (!result.Success)
            {
                // מחזירים 400 BadRequest עם אובייקט התוצאה המלא (כולל הודעת השגיאה)
                return BadRequest(result);
            }

            // אם הרישום הצליח - מחזירים 200 OK
            return Ok(result);
        }

        // ==========================================
        // Login - התחברות למערכת
        // ==========================================
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            // קריאה לשירות לאימות המשתמש ויצירת טוקן
            var result = await _userService.LoginAsync(dto);

            // אם האימות נכשל (סיסמה שגויה או משתמש לא נמצא)
            if (!result.Success)
            {
                // מחזירים 401 Unauthorized - הסטטוס הנכון לכשל באימות
                return Unauthorized(result);
            }

            // אם הכל תקין, מחזירים 200 OK עם הנתונים (הטוקן, פרטי המשתמש והתפקיד)
            return Ok(result);
        }
    }
}