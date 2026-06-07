using server.Models.DTO;

namespace server.BLL.Interfaces
{
    public interface IPurchaseService
    {
        Task<BaseResponseDTO<PurchaseDTO>> AddPurchaseAsync(PurchaseRequestDTO purchaseRequest);
        Task<BaseResponseDTO<bool>> ConfirmCartAsync(int userId);
        Task<BaseResponseDTO<IEnumerable<PurchaseDTO>>> GetUserCartAsync(int userId);
        Task<BaseResponseDTO<bool>> RemovePurchaseAsync(int id);
        Task<BaseResponseDTO<IEnumerable<PurchaseDTO>>> GetAllPurchasesAsync();
        Task<BaseResponseDTO<IEnumerable<PurchaseDTO>>> GetPurchasesReportAsync(string sortBy);
        Task<BaseResponseDTO<decimal>> GetTotalRevenueAsync();
    }
}