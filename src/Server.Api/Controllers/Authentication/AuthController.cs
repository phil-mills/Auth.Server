namespace Server.Api.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Server.Api.Attributes;
    using Server.Api.Controllers.Authentication.Models;
    using Server.Domain;

    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> logger;
        private readonly IAuthenticationService authenticationService;

        public AuthController(ILogger<AuthController> logger, IAuthenticationService authenticationService)
        {
            this.logger = logger;
            this.authenticationService = authenticationService;
        }
        
        [HttpPost]
        public async Task<IActionResult> PostAsync(AuthenticateRequest model)
        {
            var response = await authenticationService.Authenticate(model.ToDomainModel());

            if (response == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            return Ok(AuthenticateResponse.FromDomainModel(response));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAsync()
        {
            return Ok("Succesfully authorized!");
        }

        [HttpGet]
        [Route("admin")]
        [Authorize(Roles = new string[] { "Admin" })]
        public async Task<IActionResult> GetAdminAsync()
        {
            return Ok("Succesfully authorized as admin!");
        }

        [HttpGet]
        [Route("superadmin")]
        [Authorize(Roles = new string[] { "SuperAdmin" })]
        public async Task<IActionResult> GetSuperAdminAsync()
        {
            return Ok("Succesfully authorized as super admin!");
        }

        [HttpGet]
        [Route("multiple-claim-check")]
        [Authorize(Roles = new string[] { "Admin", "User" })]
        public async Task<IActionResult> GetMultipleClaimCheckAsync()
        {
            return Ok("Succesfully authorized as admin or user!");
        }
    }
}