namespace Server.Api.Controllers.Authentication.Models
{
    public class AuthenticateRequest
    {
        public string Username { get; set; }
        
        public string Password { get; set; }
        
        public Server.Domain.Authentication.Models.AuthenticateRequest ToDomainModel()
        {
            return new Server.Domain.Authentication.Models.AuthenticateRequest
            {
                Username = this.Username,
                Password = this.Password
            };
        }
    }
}