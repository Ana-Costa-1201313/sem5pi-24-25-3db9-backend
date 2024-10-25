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

        [HttpGet("doctorGetAll/{doctorId}")]
        public async Task<ActionResult<IEnumerable<OperationRequestDto>>> GetAllByDoctorId(Guid doctorId)
        {
            try
            {
                await _authService.IsAuthorized(Request, new List<string> { "Admin", "Doctor" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            var OperationRequests = await _service.GetAllByDoctorIdAsync(doctorId);

            if (OperationRequests == null || !OperationRequests.Any())
            {
                return NotFound();
            }

            return Ok(OperationRequests);
        }

        [HttpGet("doctorGetByPatientName/{doctorId}/{patientName}")]
        public async Task<ActionResult<IEnumerable<OperationRequestDto>>> GetAllByPatientNameAsDoctor(Guid doctorId, string patientName)
        {
            try
            {
                await _authService.IsAuthorized(Request, new List<string> { "Admin", "Doctor" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
            var OperationRequests = await _service.GetAllByPatientNameAsDoctorAsync(doctorId, patientName);

            if (OperationRequests == null || !OperationRequests.Any())
            {
                return NotFound();
            }

            return Ok(OperationRequests);
        }

        [HttpGet("doctorGetByPriority/{doctorId}/{priority}")]
        public async Task<ActionResult<IEnumerable<OperationRequestDto>>> GetAllByPriorityAsDoctor(Guid doctorId, string priority)
        {
            try
            {
                await _authService.IsAuthorized(Request, new List<string> { "Admin", "Doctor" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
            var OperationRequests = await _service.GetAllByPriorityAsDoctorAsync(doctorId, priority);

            if (OperationRequests == null || !OperationRequests.Any())
            {
                return NotFound();
            }

            return Ok(OperationRequests);
        }

        [HttpGet("doctorGetByOperationTypeName/{doctorId}/{opTypeName}")]
        public async Task<ActionResult<IEnumerable<OperationRequestDto>>> GetAllByOpTypeNameAsDoctor(Guid doctorId, string opTypeName)
        {
            try
            {
                await _authService.IsAuthorized(Request, new List<string> { "Admin", "Doctor" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
            var OperationRequests = await _service.GetAllByOpTypeNameAsDoctorAsync(doctorId, opTypeName);

            if (OperationRequests == null || !OperationRequests.Any())
            {
                return NotFound();
            }

            return Ok(OperationRequests);
        }

        [HttpGet("doctorGetByStatus/{doctorId}/{status}")]
        public async Task<ActionResult<IEnumerable<OperationRequestDto>>> GetAllByStatusAsDoctor(Guid doctorId, string status)
        {
            try
            {
                await _authService.IsAuthorized(Request, new List<string> { "Admin", "Doctor" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
            var OperationRequests = await _service.GetAllByStatusAsDoctorAsync(doctorId, status);

            if (OperationRequests == null || !OperationRequests.Any())
            {
                return NotFound();
            }

            return Ok(OperationRequests);
        }

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
    }
}