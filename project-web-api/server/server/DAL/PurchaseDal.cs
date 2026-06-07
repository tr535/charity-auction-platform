using Microsoft.EntityFrameworkCore;
using server.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EF_core.DAL;

namespace server.DAL
{
    public class PurchaseDal : IPurchaseDal
    {
        private readonly ChineseSaleContext _context;

        public PurchaseDal(ChineseSaleContext context)
        {
            _context = context;
        }

        // שליפה לצורכי הצגה - AsNoTracking חיוני כאן
        public async Task<List<Purchase>> GetAllPurchasesAsync()
        {
            return await _context.Set<Purchase>()
                .Include(p => p.Gift)
                .Include(p => p.User)
                .AsNoTracking()
                .ToListAsync();
        }

        // שינוי קריטי: בשליפה לעדכון (By ID) אנחנו לא משתמשים ב-NoTracking 
        // כדי ש-EF יוכל לנהל את השינוי בצורה טבעית
        public async Task<Purchase?> GetPurchaseByIdAsync(int id)
        {
            return await _context.Set<Purchase>()
                .Include(p => p.Gift)
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Purchase>> GetPurchasesByUserIdAsync(int userId)
        {
            return await _context.Set<Purchase>()
                .Include(p => p.Gift)
                .Where(p => p.UserId == userId && p.PurchaseStatus == Purchase.Status.Draft)
                .ToListAsync();
        }

        public async Task AddPurchaseAsync(Purchase purchase)
        {
            await _context.Set<Purchase>().AddAsync(purchase);
        }

        // התיקון המושלם: שימוש ב-SetValues למניעת התנגשות ב-Change Tracker
        public async Task UpdatePurchaseAsync(Purchase purchase)
        {
            var trackedEntity = _context.Set<Purchase>().Local.FirstOrDefault(e => e.Id == purchase.Id);

            if (trackedEntity != null)
            {
                // אם הישות כבר בזיכרון, נעדכן רק את הערכים שלה בלי להחליף את האובייקט
                _context.Entry(trackedEntity).CurrentValues.SetValues(purchase);
            }
            else
            {
                // אם היא לא בזיכרון, נגיד ל-EF לעקוב אחריה כ-Modified
                _context.Set<Purchase>().Update(purchase);
            }
        }

        public async Task RemovePurchaseAsync(int id)
        {
            var purchase = await _context.Set<Purchase>().FindAsync(id);
            if (purchase != null)
            {
                _context.Set<Purchase>().Remove(purchase);
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}