using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Backoffice.Domain.Shared;
using Backoffice.Domain.OperationTypes;

namespace Backoffice.Controllers
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
            var opType = await _service.GetByIdAsync(id);

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

            }
            catch (BusinessRuleValidationException e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }

        // Inactivate: api/OperationTypes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<OperationTypeDto>> SoftDelete(Guid id)
        {
            try
            {
                var cat = await _service.InactivateAsync(id);

                if (cat == null)
                {
                    return NotFound();
                }

                return Ok(cat);

            }
            catch (BusinessRuleValidationException e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchOperationType(Guid id, [FromBody] PatchOperationTypeDto operationTypeDto)
        {
            var updatedOperationType = await _service.Patch(id, operationTypeDto);

            if (updatedOperationType == null)
            {
                return NotFound();
            }

            return Ok(updatedOperationType);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<OperationTypeDto>> UpdateOperationType(Guid id, [FromBody] PutOperationTypeDto updateDto)
        {
            try
            {
                var updatedOperationType = await _service.Put(id, updateDto);

                if (updatedOperationType == null)
                {
                    return NotFound();
                }

                return Ok(updatedOperationType);

            }
            catch (BusinessRuleValidationException e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }



    }
}