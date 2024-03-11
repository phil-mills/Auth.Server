namespace Server.Api.Controllers.Authentication.Models
{
    using System;
    
    public class AuthenticateResponse
    {
        public Guid Id { get; set; }

        public string Token { get; set; }
        
        public static AuthenticateResponse FromDomainModel(Server.Domain.Authentication.Models.AuthenticateResponse model)
        {
            return new AuthenticateResponse
            {
                Id = model.Id,
                Token = model.Token
            };
        }
    }
}