namespace Server.Domain.Authentication
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using Server.Data.Repositories;
    using Server.Domain.Authentication.Models;
    using System.Threading.Tasks;
    using Server.Data.Entities;

    public class AuthenticationService : IAuthenticationService
    {
        private IConfiguration configuration;
        private readonly IUserRepository userRepository;
        private readonly IRoleRepository roleRepository;

        public AuthenticationService(IConfiguration configuration, IUserRepository userRepository, IRoleRepository roleRepository)
        {
            this.configuration = configuration;
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            var user = await this.userRepository.GetLoginAsync(model.Username, model.Password);

            if (user == null)
            {
                return null;
            }

            var roles = await this.roleRepository.GetBulkAsync(user.Roles?.Select(r => r) ?? new Guid[] {});

            var token = this.GenerateJwtToken(user, roles);

            return new AuthenticateResponse(user, token);
        }

        private string GenerateJwtToken(User user, List<Role> roles)
        {
            var key = Encoding.ASCII.GetBytes(
                configuration.GetSection("AppSettings")
                    .GetChildren()
                    .First(x => x.Key == "Secret")
                    .Value
            );
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] 
                { 
                    new Claim("id", user.Id.ToString()),
                    new Claim("username", user.Username),
                    new Claim("roles", string.Join(',', roles.Where(x => user.Roles.Any(r => r == x.Id)).Select(r => r.Name)))
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}