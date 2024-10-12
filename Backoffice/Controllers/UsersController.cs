using Microsoft.AspNetCore.Mvc;
using Backoffice.Domain.Users;
using Backoffice.Domain.Shared;

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
            var user = await _service.GetByIdAsync(new UserId(id));

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> Create(CreateUserDto dto)
        {
            try
            {
                var user = await _service.AddAsync(dto);

                return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
            }
            catch (BusinessRuleValidationException e)
            {
                return BadRequest(new { Message = e.Message });
            }


        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<UserDto>> UpdatePassword(Guid id, [FromBody] string password)
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
    }
}