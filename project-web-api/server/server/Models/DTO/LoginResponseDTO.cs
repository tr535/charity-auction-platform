namespace server.Models.DTO
{
    public class LoginResponseDTO
    {
        public string Token { get; set; }
        public string FullName { get; set; }
        public UserRole Role { get; set; }
        public DateTime Expiration { get; set; }
        // השדה שהיה חסר וגורם לבעיית הסל של כולם:
        public int Id { get; set; }
    }
}