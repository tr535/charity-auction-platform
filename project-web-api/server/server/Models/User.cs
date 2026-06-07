namespace server.Models
{  public enum UserRole { Customer, Manager }
    public class User
    {

      
        public int Id { get; set; }
        public string UserName { get; set; }    
        public string Password{ get; set; }    
        public string Email { get; set; }
        public string Phone { get; set; }
        public UserRole Role { get; set; }
        public bool IsActive { get; set; } = true; 
    }
}
