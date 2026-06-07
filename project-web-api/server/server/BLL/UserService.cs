using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using server.BLL.Interfaces;
using server.DAL;
using server.Models;
using server.Models.DTO;
using Microsoft.Extensions.Logging; // חובה עבור הלוגר
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class UserService : IUserService
{
    private readonly IUserDal _userDal;
    private readonly IConfiguration _config;
    private readonly ILogger<UserService> _logger; // הגדרת הלוגר

    public UserService(IUserDal userDal, IConfiguration config, ILogger<UserService> logger)
    {
        _userDal = userDal;
        _config = config;
        _logger = logger;
    }

    // רישום משתמש
    public async Task<BaseResponseDTO<bool>> RegisterAsync(RegisterDTO dto)
    {
        _logger.LogInformation("ניסיון רישום משתמש חדש: {Email}", dto.Email);

        var existingUser = await _userDal.GetUserByEmailAsync(dto.Email);
        if (existingUser != null)
        {
            _logger.LogWarning("רישום נכשל: המייל {Email} כבר קיים במערכת", dto.Email);
            return BaseResponseDTO<bool>.Failure("המשתמש כבר קיים במערכת.");
        }

        var user = new User
        {
            Email = dto.Email,
            UserName = dto.UserName,
            Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = UserRole.Customer,
            IsActive = true,
            Phone = dto.Phone
        };

        await _userDal.AddUserAsync(user);
        _logger.LogInformation("משתמש חדש נרשם בהצלחה: {Email}, מזהה: {Id}", user.Email, user.Id);

        return BaseResponseDTO<bool>.Ok(true, "הרישום בוצע בהצלחה!");
    }

    // התחברות
    public async Task<BaseResponseDTO<LoginResponseDTO>> LoginAsync(LoginDTO dto)
    {
        _logger.LogInformation("ניסיון התחברות עבור: {Email}", dto.Email);

        var user = await _userDal.GetUserByEmailAsync(dto.Email);

        if (user == null)
        {
            _logger.LogWarning("כישלון התחברות: משתמש עם מייל {Email} לא נמצא", dto.Email);
            return BaseResponseDTO<LoginResponseDTO>.Failure("שם משתמש או סיסמה שגויים.");
        }

        if (!user.IsActive)
        {
            _logger.LogWarning("ניסיון התחברות לחשבון מושבת: {Email}", dto.Email);
            return BaseResponseDTO<LoginResponseDTO>.Failure("חשבון המשתמש אינו פעיל.");
        }

        var validPassword = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);
        if (!validPassword)
        {
            _logger.LogWarning("סיסמה שגויה עבור משתמש: {Email}", dto.Email);
            return BaseResponseDTO<LoginResponseDTO>.Failure("שם משתמש או סיסמה שגויים.");
        }

        var expiration = DateTime.Now.AddHours(2);
        var token = GenerateJwtToken(user, expiration);

        _logger.LogInformation("משתמש {Email} התחבר בהצלחה. תפקיד: {Role}", user.Email, user.Role);

        var loginResponse = new LoginResponseDTO
        {
            Token = token,
            Expiration = expiration,
            Role = user.Role,
            FullName = user.UserName,
            Id = user.Id
        };

        return BaseResponseDTO<LoginResponseDTO>.Ok(loginResponse, "התחברת בהצלחה!");
    }

    public async Task<BaseResponseDTO<UserDTO>> GetUserByIdAsync(int id)
    {
        _logger.LogInformation("שליפת פרטי משתמש לפי מזהה: {Id}", id);
        // מימוש עתידי...
        return BaseResponseDTO<UserDTO>.Failure("לא מומש עדיין");
    }

    private string GenerateJwtToken(User user, DateTime expiration)
    {
        // לוג לצורך ניטור יצירת טוקנים (בלי להדפיס את הטוקן עצמו!)
        _logger.LogDebug("מייצר JWT Token עבור משתמש {Id}", user.Id);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var keyBytes = Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? "DefaultVerySecretKey1234567890123456");
        var key = new SymmetricSecurityKey(keyBytes);
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: expiration,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}