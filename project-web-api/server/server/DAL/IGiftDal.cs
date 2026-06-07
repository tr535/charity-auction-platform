using server.Models;

namespace server.DAL
{
    public interface IGiftDal
    {
        IQueryable<Gift> GetGiftsQueryable();
        Task<List<Gift>> GetAllGiftsAsync(); // הוספת הפונקציה הזו
        Task<Gift?> GetGiftByIdAsync(int id);
        Task AddGiftAsync(Gift gift);
        Task UpdateGiftAsync(Gift gift);
        Task RemoveGiftAsync(int id);
        Task SaveChangesAsync();
    }
}