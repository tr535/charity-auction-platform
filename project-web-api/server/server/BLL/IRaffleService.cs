using server.Models.DTO;

namespace server.BLL.Interfaces
{
    public interface IRaffleService
    {
        // ביצוע הגרלה למתנה ספציפית
        Task<BaseResponseDTO<RaffleResultDto>> ExecuteDrawAsync(int giftId);

        // חישוב הכנסות כולל
        Task<BaseResponseDTO<decimal>> GetTotalIncomeAsync();

        // דוח זוכים
        Task<BaseResponseDTO<List<RaffleResultDto>>> GetWinnersReportAsync();
    }
}