using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using HealthcareApp.Domain.Shared;
using HealthcareApp.Domain.Specializations;

namespace HealthcareApp.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class SpecializationsController : ControllerBase
    {
        private readonly SpecializationService _service;

        public SpecializationsController(SpecializationService service)
        {
            _service = service;
        }

        // GET: api/Specializations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SpecializationDto>>> GetAll()
        {
            return await _service.GetAllAsync();
        }

        // GET: api/Specializations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SpecializationDto>> GetGetById(Guid id)
        {
            var spec = await _service.GetByIdAsync(new SpecializationId(id));

            if (spec == null)
            {
                return NotFound();
            }

            return spec;
        }

        // POST: api/Specializations
        [HttpPost]
        public async Task<ActionResult<SpecializationDto>> Create(CreatingSpecializationDto dto)
        {
            try
            {
                var spec = await _service.AddAsync(dto);

                return CreatedAtAction(nameof(GetGetById), new { id = spec.Id }, spec);

            }
            catch (BusinessRuleValidationException e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }


        // Inactivate: api/Specializations/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<SpecializationDto>> SoftDelete(Guid id)
        {
            var spec = await _service.InactivateAsync(new SpecializationId(id));

            if (spec == null)
            {
                return NotFound();
            }

            return Ok(spec);
        }
    }
}