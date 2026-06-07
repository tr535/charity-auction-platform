namespace server.Models
{
    public class Donor
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string FamilyName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // הוספת שדה טלפון למודל כדי שישמר ב-DB
        public string? Phone { get; set; }

        // שדה עזר שמחבר את השם הפרטי ושם המשפחה (לא נשמר ב-DB)
        public string Name => $"{FirstName} {FamilyName}".Trim();

        public virtual ICollection<Gift> Gifts { get; set; } = new List<Gift>();
    }
}