using server.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.DAL
{
    public interface IDonorDal
    {
        // הפונקציה החדשה לסינונים יעילים
        IQueryable<Donor> GetDonorsQueryable();

        Task<List<Donor>> GetAllDonorsAsync();
        Task<Donor?> GetDonorByIdAsync(int id);
        Task AddDonorAsync(Donor donor);
        Task UpdateDonorAsync(Donor donor);
        Task RemoveDonorAsync(int id);
        Task SaveChangesAsync();
    }
}