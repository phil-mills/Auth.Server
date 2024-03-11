namespace Server.Domain.Authentication.Models
{    
    public class AuthenticateRequest
    {
        public string Username { get; set; }
        
        public string Password { get; set; }
    }
}