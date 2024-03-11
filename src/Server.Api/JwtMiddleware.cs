namespace Server.Api
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.Extensions.Configuration;
    using Server.Domain;
    using Server.Data.Entities;

    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private IConfiguration configuration;

        public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            this.configuration = configuration;
        }

        public async Task Invoke(HttpContext context, IAuthenticationService authenticationService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                this.AttachUserToContext(context, authenticationService, token);
            }

            await _next(context);
        }

        private void AttachUserToContext(HttpContext context, IAuthenticationService authenticationService, string token)
        {
            try
            {
                var key = Encoding.ASCII.GetBytes(
                    configuration.GetSection("AppSettings")
                        .GetChildren()
                        .First(x => x.Key == "Secret")
                        .Value
                );
                
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                context.Items["User"] = new
                {
                    Id = Guid.Parse(jwtToken.Claims.First(x => x.Type == "id").Value),
                    Username = jwtToken.Claims.First(x => x.Type == "username").Value,
                    Roles = jwtToken.Claims.First(x => x.Type == "roles").Value.Split(',')
                    .Select(role => new Role { Name = role })
                };
            }
            catch { }
        }
    }
}