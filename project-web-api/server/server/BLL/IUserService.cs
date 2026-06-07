using server.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace server.BLL.Interfaces
{
    public interface IUserService
    {
        // ברישום: אנחנו מחזירים BaseResponseDTO<bool> כדי לסמן הצלחה/כישלון
        Task<BaseResponseDTO<bool>> RegisterAsync(RegisterDTO dto);

        // בלוגין: אנחנו עוטפים את ה-LoginResponseDTO (שמכיל טוקן וכו') בתוך ה-BaseResponse
        Task<BaseResponseDTO<LoginResponseDTO>> LoginAsync(LoginDTO dto);

        // אופציונלי: שליפת פרטי משתמש לפי ID
        Task<BaseResponseDTO<UserDTO>> GetUserByIdAsync(int id);
    }
}