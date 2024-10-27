using Microsoft.AspNetCore.Mvc;
using Backoffice.Domain.OperationRequests;
using Backoffice.Domain.Shared;
using Backoffice.Domain.Staffs;

namespace Backoffice.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class OperationRequestController : ControllerBase
    {
        private readonly OperationRequestService _service;
        private readonly AuthService _authService;
        public OperationRequestController(OperationRequestService service, AuthService authService)
        {
            _service = service;
            _authService = authService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OperationRequestDto>>> GetAll()
        {
            try
            {
                await _authService.IsAuthorized(Request, new List<string> { "Admin" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            var opReqList = await _service.GetAllAsync();

            if (opReqList == null || opReqList.Count == 0)
            {
                return NoContent();
            }

            return Ok(opReqList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OperationRequestDto>> GetById(Guid id)
        {
            var OperationRequest = await _service.GetByIdAsync(id);

            if (OperationRequest == null)
            {
                return NotFound();
            }

            return OperationRequest;
        }

        [HttpGet("list/{doctorId}")]
        public async Task<ActionResult<IEnumerable<OperationRequestDto>>> GetByFilter(string doctorId, [FromQuery] string parameter, [FromQuery] string value)
        {
            try
            {
                await _authService.IsAuthorized(Request, new List<string> { "Admin", "Doctor" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            List<OperationRequestDto> OperationRequests = new List<OperationRequestDto>();

            try
            {
                switch (parameter)
                {
                    case "patient":
                        OperationRequests = await _service.GetAllByPatientIdAsDoctorAsync(doctorId, value);
                        break;
                    case "priority":
                        OperationRequests = await _service.GetAllByPriorityAsDoctorAsync(doctorId, value);
                        break;
                    case "operation type":
                        OperationRequests = await _service.GetAllByOpTypeIdAsDoctorAsync(doctorId, value);
                        break;
                    case "status":
                        OperationRequests = await _service.GetAllByStatusAsDoctorAsync(doctorId, value);
                        break;
                    case "":
                        OperationRequests = await _service.GetAllByDoctorIdAsync(doctorId);
                        break;
                    default:
                        return BadRequest(new { Message = "Invalid parameter!" });
                }

                //var OperationRequests = await _service.GetAllByDoctorEmailAsync(doctorEmail);

                if (OperationRequests == null || !OperationRequests.Any())
                {
                    return NotFound();
                }

                return Ok(OperationRequests);
            }
            catch (BusinessRuleValidationException e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }

        [HttpGet("doctorGetAll/{doctorId}")]
        public async Task<ActionResult<IEnumerable<OperationRequestDto>>> GetAllByDoctorId(string doctorId)
        {
            try
            {
                await _authService.IsAuthorized(Request, new List<string> { "Admin", "Doctor" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            try
            {
                var OperationRequests = await _service.GetAllByDoctorIdAsync(doctorId);

                if (OperationRequests == null || !OperationRequests.Any())
                {
                    return NotFound();
                }

                return Ok(OperationRequests);
            }
            catch (BusinessRuleValidationException e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }/*

        [HttpGet("doctorGetByPatientEmail/{doctorEmail}/{patientEmail}")]
        public async Task<ActionResult<IEnumerable<OperationRequestDto>>> GetAllByPatientEmailAsDoctor(string doctorEmail, string patientEmail)
        {
            try
            {
                await _authService.IsAuthorized(Request, new List<string> { "Admin", "Doctor" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
            try
            {
                var OperationRequests = await _service.GetAllByPatientEmailAsDoctorAsync(doctorEmail, patientEmail);

                if (OperationRequests == null || !OperationRequests.Any())
                {
                    return NotFound();
                }

                return Ok(OperationRequests);
            }
            catch (BusinessRuleValidationException e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }

        [HttpGet("doctorGetByPriority/{doctorEmail}/{priority}")]
        public async Task<ActionResult<IEnumerable<OperationRequestDto>>> GetAllByPriorityAsDoctor(string doctorEmail, string priority)
        {
            try
            {
                await _authService.IsAuthorized(Request, new List<string> { "Admin", "Doctor" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            try
            {
                var OperationRequests = await _service.GetAllByPriorityAsDoctorAsync(doctorEmail, priority);

                if (OperationRequests == null || !OperationRequests.Any())
                {
                    return NotFound();
                }

                return Ok(OperationRequests);
            }
            catch (BusinessRuleValidationException e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }

        [HttpGet("doctorGetByOperationTypeName/{doctorEmail}/{opTypeName}")]
        public async Task<ActionResult<IEnumerable<OperationRequestDto>>> GetAllByOpTypeNameAsDoctor(string doctorEmail, string opTypeName)
        {
            try
            {
                await _authService.IsAuthorized(Request, new List<string> { "Admin", "Doctor" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
            try
            {
                var OperationRequests = await _service.GetAllByOpTypeNameAsDoctorAsync(doctorEmail, opTypeName);

                if (OperationRequests == null || !OperationRequests.Any())
                {
                    return NotFound();
                }

                return Ok(OperationRequests);
            }
            catch (BusinessRuleValidationException e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }

        [HttpGet("doctorGetByStatus/{doctorEmail}/{status}")]
        public async Task<ActionResult<IEnumerable<OperationRequestDto>>> GetAllByStatusAsDoctor(string doctorEmail, string status)
        {
            try
            {
                await _authService.IsAuthorized(Request, new List<string> { "Admin", "Doctor" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
            try
            {
                var OperationRequests = await _service.GetAllByStatusAsDoctorAsync(doctorEmail, status);

                if (OperationRequests == null || !OperationRequests.Any())
                {
                    return NotFound();
                }

                return Ok(OperationRequests);
            }
            catch (BusinessRuleValidationException e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }*/

        [HttpPost]
        public async Task<ActionResult<OperationRequestDto>> Create(CreateOperationRequestDto dto)
        {
            try
            {
                await _authService.IsAuthorized(Request, new List<string> { "Doctor", "Admin" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            try
            {
                var opRequest = await _service.AddAsync(dto);

                return CreatedAtAction(nameof(GetById), new { id = opRequest.Id }, opRequest);
            }
            catch (BusinessRuleValidationException e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<OperationRequestDto>> Update(Guid id, [FromBody] EditOperationRequestDto dto)
        {
            try
            {
                await _authService.IsAuthorized(Request, new List<string> { "Admin", "Doctor" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            try
            {
                var opRequest = await _service.PatchAsync(id, dto );

                if (opRequest == null)
                {
                    return NotFound();
                }

                return Ok(opRequest);
            }
            catch (BusinessRuleValidationException e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<OperationRequestDto>> Delete(Guid id)
        {
            try
            {
                await _authService.IsAuthorized(Request, new List<string> { "Admin", "Doctor" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            var opRequest = await _service.DeleteAsync(id);

            if (opRequest == null)
            {
                return NotFound();
            }

            return Ok(opRequest);
        }
    }
}