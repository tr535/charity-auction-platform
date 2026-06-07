using server.Models.DTO;

namespace server.BLL.Interfaces
{
    public interface IDonorService
    {
        Task<BaseResponseDTO<IEnumerable<DonorDTO>>> GetAllDonorsAsync(string? name, string? email);
        Task<BaseResponseDTO<DonorDTO>> GetDonorByIdAsync(int id);
        Task<BaseResponseDTO<DonorDTO>> AddDonorAsync(DonorDTO donorDTO);
        Task<BaseResponseDTO<bool>> UpdateDonorAsync(int id, DonorDTO donorDTO);
        Task<BaseResponseDTO<bool>> RemoveDonorAsync(int id);
    }
}