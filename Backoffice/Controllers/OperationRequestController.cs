using Microsoft.AspNetCore.Mvc;
using Backoffice.Domain.OperationRequests;
using Backoffice.Domain.Shared;

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