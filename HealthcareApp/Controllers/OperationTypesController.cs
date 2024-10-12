using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using HealthcareApp.Domain.Shared;
using HealthcareApp.Domain.OperationTypes;

namespace HealthcareApp.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class OperationTypesController : ControllerBase
    {
        private readonly OperationTypeService _service;

        public OperationTypesController(OperationTypeService service)
        {
            _service = service;
        }

        // GET: api/OperationTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OperationTypeDto>>> GetAll()
        {
            return await _service.GetAllAsync();
        }

        // GET: api/OperationTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OperationTypeDto>> GetGetById(Guid id)
        {
            var opType = await _service.GetByIdAsync(new OperationTypeId(id));

            if (opType == null)
            {
                return NotFound();
            }

            return opType;
        }

        // POST: api/OperationTypes
        [HttpPost]
        public async Task<ActionResult<OperationTypeDto>> Create(CreatingOperationTypeDto dto)
        {
            try
            {
                var opType = await _service.AddAsync(dto);

                return CreatedAtAction(nameof(GetGetById), new { id = opType.Id }, opType);

            }catch(BusinessRuleValidationException e){
                return BadRequest(new { Message = e.Message });
            }
            }
    }
}