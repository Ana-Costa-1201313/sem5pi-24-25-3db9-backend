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
        public OperationRequestController(OperationRequestService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OperationRequestDto>>> GetAll()
        {
            return await _service.GetAllAsync();
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
                var OperationRequest = await _service.AddAsync(dto);

                return CreatedAtAction(nameof(GetById), new { id = OperationRequest.Id }, OperationRequest);
            }
            catch (BusinessRuleValidationException e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }
    }
}