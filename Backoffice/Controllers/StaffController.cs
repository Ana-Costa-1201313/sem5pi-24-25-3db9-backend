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

        private readonly AuthService _authService;

        public StaffController(StaffService service, AuthService authService)
        {
            _service = service;
            _authService = authService;
        }

        [HttpGet]
        public async Task<ActionResult<List<StaffDto>>> GetAll(
            [FromQuery] string name,
            [FromQuery] string email,
            [FromQuery] string specialization,
            [FromQuery] int pageNum = 1,
            [FromQuery] int pageSize = 5
        )
        {
            try
            {
                await _authService.IsAuthorized(Request, new List<string> { "Admin" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            List<StaffDto> staffList;

            if (Request.Query.ContainsKey("name") || Request.Query.ContainsKey("email") || Request.Query.ContainsKey("specialization"))
            {
                staffList = await _service.FilterStaffAsync(name, email, specialization, pageNum, pageSize);
            }
            else
            {
                staffList = await _service.GetAllAsync(pageNum, pageSize);
            }

            if (staffList == null || staffList.Count == 0)
            {
                return NoContent();
            }

            return Ok(staffList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StaffDto>> GetById(Guid id)
        {
            try
            {
                await _authService.IsAuthorized(Request, new List<string> { "Admin" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            StaffDto staff = await _service.GetByIdAsync(id);

            if (staff == null)
            {
                return NotFound();
            }

            return Ok(staff);
        }

        [HttpPost]
        public async Task<ActionResult<StaffDto>> Create(CreateStaffDto dto)
        {
            try
            {
                await _authService.IsAuthorized(Request, new List<string> { "Admin" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            try
            {
                StaffDto staff = await _service.AddAsync(dto);

                return CreatedAtAction(nameof(GetById), new { id = staff.Id }, staff);
            }
            catch (BusinessRuleValidationException e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<StaffDto>> Deactivate(Guid id)
        {
            try
            {
                await _authService.IsAuthorized(Request, new List<string> { "Admin" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            try
            {
                StaffDto staff = await _service.Deactivate(id);

                if (staff == null)
                {
                    return NotFound();
                }

                return Ok(staff);
            }
            catch (BusinessRuleValidationException e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<StaffDto>> Update(Guid id, EditStaffDto dto)
        {
            try
            {
                await _authService.IsAuthorized(Request, new List<string> { "Admin" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            if (id != dto.Id)
            {
                return BadRequest(new { Message = "The staff Id does not match the header!" });
            }

            try
            {
                StaffDto staff = await _service.UpdateAsync(dto);

                if (staff == null)
                {
                    return NotFound();
                }

                return Ok(staff);
            }
            catch (BusinessRuleValidationException e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<StaffDto>> PartialUpdate(Guid id, EditStaffDto dto)
        {
            try
            {
                await _authService.IsAuthorized(Request, new List<string> { "Admin" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            if (id != dto.Id)
            {
                return BadRequest(new { Message = "The staff Id does not match the header!" });
            }

            try
            {
                StaffDto staff = await _service.PartialUpdateAsync(dto);

                if (staff == null)
                {
                    return NotFound();
                }

                return Ok(staff);
            }
            catch (BusinessRuleValidationException e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }
    }
}