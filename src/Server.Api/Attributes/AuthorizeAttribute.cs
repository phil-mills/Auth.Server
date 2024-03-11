namespace Server.Api.Attributes
{
    using System;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Server.Data.Entities;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public string[] Roles { get; set; }

        public AuthorizeAttribute() 
        { 
            Roles = new string[] { };
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.Items["User"];

            if (user == null)
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }

            if (Roles.Count() > 0)
            {
                var hasRole = false;
                var roles = (IEnumerable<Role>) user.GetType().GetProperty("Roles").GetValue(user);

                foreach (var role in Roles)
                {
                    if (roles.Any(r => r.Name.ToLower() == role.ToLower()))
                    {
                        hasRole = true;
                        break;
                    }
                }

                if (!hasRole)
                {
                    context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
                }
            }
        }
    }
}