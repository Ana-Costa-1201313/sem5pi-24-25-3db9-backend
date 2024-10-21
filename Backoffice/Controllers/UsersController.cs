using Microsoft.AspNetCore.Mvc;
using Backoffice.Domain.Users;
using Backoffice.Domain.Shared;
using System.Configuration;

namespace Backoffice.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _service;

        public UsersController(UserService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
        {
            return await _service.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetById(Guid id)
        {
            var user = await _service.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> Create(CreateUserDto dto)
        {

            List<String> roles = new List<String> { "Admin" };

            // verificar header -> enviar header token para auth -> confirmar validade -> continuar
            if (!Request.Headers.TryGetValue("Authorization", out var tokenHeader))
            {
                return BadRequest("Authorization header is missing");
            }
            checkHeader(roles, tokenHeader);
            
            try
            {
                var user = await _service.AddAsync(dto);

                return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
            }
            catch (BusinessRuleValidationException e)
            {
                return BadRequest(new { Message = e.Message });
            }
            catch (ConfigurationErrorsException e)
            {
                return StatusCode(500, new { message = e.Message });
            }
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<UserDto>> UpdatePassword(Guid id, [FromQuery] string password)
        {
            try
            {
                var user = await _service.UpdatePassword(id, password);

                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (BusinessRuleValidationException e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }

        private async void checkHeader(List<String> roles, String token) 
        {
            LoginDTO loginDTO = await _service.validateAuthorization(token);

            if (loginDTO == null || !roles.Contains(loginDTO.role)) throw new UnauthorizedAccessException("User does not have necessary roles!");
        }
    }
}