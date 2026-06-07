namespace server.Models.DTO
{
    public class RaffleResultDto
    {
        public string? WinnerName { get; set; }

        // שדה חובה לשליחת המייל ב-EmailJS
        public string? WinnerEmail { get; set; }

        public int GiftId { get; set; }

        // שדה חובה כדי שהמייל יכיל את שם הפרס
        public string? GiftName { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}