using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Backoffice.Domain.Shared;
using Backoffice.Domain.Specializations;

namespace Backoffice.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class SpecializationsController : ControllerBase
    {
        private readonly SpecializationService _service;
        private readonly AuthService _authService;


        public SpecializationsController(SpecializationService service, AuthService authService)
        {
            _service = service;
            _authService = authService;
        }

        // GET: api/Specializations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SpecializationDto>>> GetAll()
        {
            try
            {
                await _authService.IsAuthorized(Request, new List<string> { "Admin" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return await _service.GetAllAsync();
        }

        // GET: api/Specializations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SpecializationDto>> GetGetById(Guid id)
        {
            try
            {
                await _authService.IsAuthorized(Request, new List<string> { "Admin" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            var spec = await _service.GetByIdAsync(id);

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
                await _authService.IsAuthorized(Request, new List<string> { "Admin" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

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

        [HttpDelete("{id}")]
        public async Task<ActionResult<SpecializationDto>> SoftDelete(Guid id)
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
    }
}