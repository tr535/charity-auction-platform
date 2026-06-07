
namespace server.Models.DTO
{
    public class RegisterDTO



    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; } // ודאי אם קראת לזה UserName או Name ב-Service
        public string Phone { get; set; }

    }
}
