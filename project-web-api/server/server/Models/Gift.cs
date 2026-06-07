namespace server.Models
{
    public class Gift
    {
        public int Id { get; set; }
        public string NameGift { get; set; }

        public int DonorId { get; set; }
        public Donor Donor { get; set; }
        public string Category { get; set; }

        public int PriceTicket { get; set; }
        public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
        // האם ההגרלה כבר התבצעה?
        public bool IsDrawn { get; set; } = false;

        // מי המשתמש שזכה (יכול להיות ריק עד שמתבצעת הגרלה)
        public int? WinnerUserId { get; set; }
        public User? WinnerUser { get; set; }
    }
}