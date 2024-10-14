using Microsoft.AspNetCore.Mvc;
using Auth.Domain.Users;
using Auth.Domain.Users.DTO;
using Microsoft.IdentityModel.Tokens;

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

        [HttpGet]
        public ActionResult<string> Login()
        {
            return Ok("boas");
        }

        [HttpPost("{loginDto}")]
        public async Task<ActionResult<UserDTO>> login(LoginDTO loginDto)
        {

            try
            {
                if (loginDto.jwt != null) { 
                    if(await _service.validToken(loginDto.jwt))
                        return Ok(new UserDTO { jwt = loginDto.jwt});
                }
                    //    //else if (loginDto.googleCredentials != null)
                    //    //    return Ok(await _service.loginGoogle(loginDto.googleCredentials));
                else if (loginDto.username != null && loginDto.password != null)
                    return Ok(await _service.login(loginDto.username, loginDto.password));
                //    return BadRequest(new { Message = "Body no formato errado." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "mau boas" });
            }

            return null;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LoginDTO>> teste(string id) 
        {

            var login = new LoginDTO
            {
                jwt = _service.CreateToken(new Domain.Users.User("ricardo1"))
            };

            return Ok(login);
        }

        [HttpPost("validate")]
        public async Task<ActionResult<LoginDTO>> validateToken(LoginDTO loginDTO) {

            if (loginDTO == null || string.IsNullOrEmpty(loginDTO.jwt))
            {
                return BadRequest(new { Message = "Invalid token data." });
            }

            if (await _service.validToken(loginDTO.jwt))
            {
                return Ok(loginDTO);
            }
            else
            {
                return Unauthorized(new { Message = "Invalid or expired token." });
            }
        }



    }
}




        