using server.Models;

namespace server.DAL
{
    public interface IPurchaseDal
    {
        Task<List<Purchase>> GetAllPurchasesAsync();
        Task<Purchase?> GetPurchaseByIdAsync(int id);
        Task<List<Purchase>> GetPurchasesByUserIdAsync(int userId);
        Task AddPurchaseAsync(Purchase purchase);
        Task UpdatePurchaseAsync(Purchase purchase);
        Task RemovePurchaseAsync(int id);
        Task SaveChangesAsync();
    }
}