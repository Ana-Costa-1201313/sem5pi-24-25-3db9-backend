using Microsoft.AspNetCore.Mvc;
using Auth.Domain.Users;
using Auth.Domain.Users.DTO;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.AspNetCore.Authorization;

namespace Auth.Controllers
{

    [Route("Auth/[Controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private readonly LoginService _service;

        public LoginController(LoginService service) // IAppService
        {
            _service = service;
        }


        [HttpPost("loginDto")]
        public async Task<ActionResult<LoginDTO>> login( [FromBody] LoginDTO loginDto)
        {

            try
            {
                if (loginDto.jwt != null)
                {
                    if (await _service.validToken(loginDto.jwt))
                        return Ok(loginDto);
                }
                else if (loginDto.googleCredentials != null)
                    return Ok(await _service.loginGoogle(loginDto.googleCredentials));
                else if (loginDto.username != null && loginDto.password != null)
                {
                    var result = await _service.login(loginDto.username, loginDto.password);
                    if (result == null) return BadRequest(new { Message = "LoginController - loginDto é null" });
                    return Ok(result);
                }
                //}else if (loginDto.username != null && loginDto.password != null)
                //{
                //    var result = await _service.login(loginDto.username, loginDto.password);
                //    return Ok(result);
                //}
                    //return BadRequest(new { Message = "mau boas." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return BadRequest(new { Message = "An error occurred during login." }); 
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<String> loginGoogle(int num)
        {
            var user = this.User;

            return "value";
        }

        //[HttpGet("{id}")]
        //public async Task<ActionResult<LoginDTO>> teste(string id) 
        //{
        //    var l = new LoginDTO
        //    {
        //        username = "ricardo",
        //        role = "student"
        //    };

        //    var login = new LoginDTO
        //    {
        //        //jwt = _service.CreateToken(new Domain.Users.User("ricardo1", "student"))
        //        jwt = _service.CreateToken(l)
        //    };

        //    return Ok(login);
        //}

        [HttpPost("validate")]
        public async Task<ActionResult<LoginDTO>> validateToken(LoginDTO loginDTO) {

            try
            {
                // Read the token from the request header
                if (!Request.Headers.TryGetValue("Authorization", out var tokenHeader))
                {
                    return BadRequest("Authorization header is missing");
                }

                // Remove the 'Bearer ' prefix from the token
                var token = tokenHeader.ToString().Replace("Bearer ", string.Empty);

                // Use the validateToken method to check the token's validity
                bool valid = await _service.validToken(loginDTO.jwt);

                // Call the isTokenGood() method to further validate the token (assuming this method is available)
                if (valid)
                {
                    loginDTO = _service.ExtractLoginDTOFromToken(loginDTO);
                    return Ok(loginDTO);
                }
                else
                {
                    return Unauthorized("Invalid token");
                }
            }
            catch (Exception ex)
            {
                // Return a generic error message for any exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



    }
}




        