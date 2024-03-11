namespace Server.Domain
{
    using System.Threading.Tasks;
    using Server.Domain.Authentication.Models;

    public interface IAuthenticationService
    {
        public Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
    }
}