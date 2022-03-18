using Microsoft.AspNetCore.Mvc;
using P320.AuthenticationService.Contracts;
using P320.AuthenticationService.Models;

namespace P320.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("token")]
        public IActionResult Token([FromBody] CredentialModel credentialModel)
        {
            var token = _authService.GetToken(credentialModel);

            return Ok(token);
        }
    }
}
