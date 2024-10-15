using Microsoft.AspNetCore.Mvc;
using Backoffice.Domain.Users;
using Microsoft.IdentityModel.Tokens;
using Backoffice.Services;

namespace Backoffice.Controllers
{

    [Route("Backoffice/[Controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private readonly LogInServices _service;

        public LoginController(LogInServices service) 
        {
            _service = service;
        }

        [HttpGet("userExists")]
        public async Task<IActionResult> UserExists([FromQuery] string username, [FromQuery] string password)
        {
            // Validate parameters  present
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return BadRequest("Username and password must be provided.");
            }

            // query database
            bool userExists = await CheckUserCredentials(username, password);

            if (userExists)
            {
                return Ok("User exists and credentials are valid.");
            }
            else
            {
                return Unauthorized("Invalid username or password.");
            }
        }

        private async Task<bool> CheckUserCredentials(string username, string password) { 
            return await _service.CheckUserCredentials1(username, password);
        }
    }
}