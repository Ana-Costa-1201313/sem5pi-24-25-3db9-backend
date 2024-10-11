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

            //try
            //{
            //    if (loginDto.jwt != null)
            //        return Ok(await _service.jwtValido(loginDto.jwt));
            //    //else if (loginDto.googleCredentials != null)
            //    //    return Ok(await _service.loginGoogle(loginDto.googleCredentials));
            //    else if (loginDto.userName != null && loginDto.password != null)
            //        return Ok(await _service.login(loginDto.userName, loginDto.password));
            //    return BadRequest(new { Message = "Body no formato errado." });
            //}
            //catch (Exception ex)
            //{
            return BadRequest(new { Message = "mau boas" });
            //}
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SecurityToken>> teste(string id) 
        {

            var login = new LoginDTO
            {
                //jwt = _service.CreateToken(new Domain.Users.User("ricardo")),
                a = 5
            };


            //return Ok(login);
            return _service.CreateToken(new Domain.Users.User("ricardo"));
        }

    }
}




        