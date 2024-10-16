using Microsoft.AspNetCore.Mvc;
using Backoffice.Domain.Staffs;
using Backoffice.Domain.Shared;

namespace Backoffice.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly StaffService _service;
        public StaffController(StaffService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StaffDto>>> GetAll()
        {
            return await _service.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StaffDto>> GetById(Guid id)
        {
            var staff = await _service.GetByIdAsync(id);

            if (staff == null)
            {
                return NotFound();
            }

            return staff;
        }

        [HttpPost]
        public async Task<ActionResult<StaffDto>> Create(CreateStaffDto dto)
        {
            try
            {
                var staff = await _service.AddAsync(dto);

                return CreatedAtAction(nameof(GetById), new { id = staff.Id }, staff);
            }
            catch (BusinessRuleValidationException e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }
    }
}