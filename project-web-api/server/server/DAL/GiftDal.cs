using EF_core.DAL;
using Microsoft.EntityFrameworkCore;
using server.Models;

namespace server.DAL
{
    public class GiftDal : IGiftDal
    {
        private readonly ChineseSaleContext _context;

        public GiftDal(ChineseSaleContext context)
        {
            _context = context;
        }

        public IQueryable<Gift> GetGiftsQueryable()
        {
            return _context.Gifts
                .Include(g => g.Donor)
                .Include(g => g.WinnerUser)
                .AsNoTracking()
                .AsQueryable();
        }

        public async Task<List<Gift>> GetAllGiftsAsync()
        {
            return await GetGiftsQueryable().ToListAsync();
        }

        public async Task<Gift?> GetGiftByIdAsync(int id)
        {
            return await _context.Gifts
                .Include(g => g.Donor)
                .Include(g => g.WinnerUser)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task AddGiftAsync(Gift gift)
        {
            await _context.Gifts.AddAsync(gift);
        }

        public async Task UpdateGiftAsync(Gift gift)
        {
            var trackedEntity = _context.Gifts.Local.FirstOrDefault(e => e.Id == gift.Id);

            if (trackedEntity != null)
            {
                // במקום להחליף את הישות, אנחנו רק מעדכנים את הערכים שלה
                _context.Entry(trackedEntity).CurrentValues.SetValues(gift);
            }
            else
            {
                _context.Gifts.Update(gift);
            }
        }

        public async Task RemoveGiftAsync(int id)
        {
            var gift = await _context.Gifts.FindAsync(id);
            if (gift != null)
            {
                _context.Gifts.Remove(gift);
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}