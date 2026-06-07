using EF_core.DAL;
using Microsoft.EntityFrameworkCore;
using server.Models;

namespace server.DAL
{
    public class DonorDal : IDonorDal
    {
        private readonly ChineseSaleContext _context;

        public DonorDal(ChineseSaleContext context) => _context = context;

        public IQueryable<Donor> GetDonorsQueryable()
        {
            return _context.Donors.Include(d => d.Gifts).AsQueryable();
        }

        public async Task<List<Donor>> GetAllDonorsAsync()
        {
            return await _context.Donors.Include(d => d.Gifts).ToListAsync();
        }

        public async Task<Donor?> GetDonorByIdAsync(int id)
        {
            return await _context.Donors.Include(d => d.Gifts).FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task AddDonorAsync(Donor donor) => await _context.Donors.AddAsync(donor);

        public async Task UpdateDonorAsync(Donor donor) => _context.Donors.Update(donor);

        public async Task RemoveDonorAsync(int id)
        {
            var donor = await GetDonorByIdAsync(id);
            if (donor != null) _context.Donors.Remove(donor);
        }

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}