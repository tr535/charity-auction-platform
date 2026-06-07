
using server.Models;
using Microsoft.EntityFrameworkCore;

namespace EF_core.DAL
{
    public class ChineseSaleContext : DbContext
    {
        public DbSet<Gift> Gifts { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Donor> Donors { get; set; }

        public ChineseSaleContext(DbContextOptions<ChineseSaleContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // הגדרת המרות לטקסט עבור Enums
            modelBuilder.Entity<Purchase>()
                .Property(p => p.PurchaseStatus)
                .HasConversion<string>();

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>();

            // --- הוספת מנהל ראשון למערכת (Seed Data) ---
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1, // חובה לתת ID ידני ב-Seed Data
                UserName = "מנהל",
                Email = "m@m.com",
                // יצירת סיסמה מוצפנת מראש (למשל עבור 123456)
                Password = "$2a$11$mURkDYh2uXPflWwbM1Hw9eftkHn5Aqk5lnX0FCDC3oj.x505BQkA.",
                Role = UserRole.Manager, // וודאי שקיים ערך כזה ב-Enum שלך
                IsActive = true,
                Phone = "050-1234567"
            });
        }

    }
}
