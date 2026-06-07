using server.DTOs;        // עבור GiftDto
using server.Models.DTO;  // עבור BaseResponseDTO
using System.Collections.Generic;
using System.Threading.Tasks;

namespace server.BLL.Interfaces
{
    public interface IGiftService
    {
        Task<BaseResponseDTO<IEnumerable<GiftDto>>> GetAllGiftsAsync(
            string? name,
            string? category,
            string? donorName,
            int? minPurchasers,
            string? sortBy);

        Task<BaseResponseDTO<GiftDto>> GetGiftByIdAsync(int id);
        Task<BaseResponseDTO<GiftDto>> AddGiftAsync(GiftDto giftDto);
        Task<BaseResponseDTO<bool>> UpdateGiftAsync(int id, GiftDto giftDto);
        Task<BaseResponseDTO<bool>> RemoveGiftAsync(int id);
    }
}