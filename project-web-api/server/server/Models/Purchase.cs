using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    public class Purchase
    {
        public enum Status { Draft, Confirmed, Winner }

        public int Id { get; set; }

        // השדה החדש שפתר את השגיאה ב-AutoMapper
        [Required]
        public DateTime PurchaseDate { get; set; } = DateTime.Now;

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [Required]
        public int GiftId { get; set; }

        [ForeignKey("GiftId")]
        public virtual Gift Gift { get; set; }

        [Required]
        public Status PurchaseStatus { get; set; } = Status.Draft;
    }
}