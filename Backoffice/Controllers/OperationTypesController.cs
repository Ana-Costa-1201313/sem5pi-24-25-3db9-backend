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
        private readonly IExternalApiServices _authService;

        public OperationTypesController(OperationTypeService service, IExternalApiServices authService)
        {
            _service = service;
            _authService = authService;
        }

        // GET: api/OperationTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OperationTypeDto>>> GetAll()
        {

            List<String> roles = new List<String> { "Admin" };

            if (!Request.Headers.TryGetValue("Authorization", out var tokenHeader))
            {
                return BadRequest("Authorization header is missing");
            }
            try
            {

                if (!await _authService.checkHeader(roles, tokenHeader))
                {
                    return BadRequest("User not autenticated");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            var opTypeList = await _service.GetAllAsync();

            if (opTypeList == null || !opTypeList.Any())
            {
                return NoContent();
            }

            return Ok(opTypeList);
        }

        // GET: api/OperationTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OperationTypeDto>> GetGetById(Guid id)
        {

            List<String> roles = new List<String> { "Admin" };

            if (!Request.Headers.TryGetValue("Authorization", out var tokenHeader))
            {
                return BadRequest("Authorization header is missing");
            }
            try
            {

                if (!await _authService.checkHeader(roles, tokenHeader))
                {
                    return BadRequest("User not autenticated");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

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

            List<String> roles = new List<String> { "Admin" };

            if (!Request.Headers.TryGetValue("Authorization", out var tokenHeader))
            {
                return BadRequest("Authorization header is missing");
            }
            try
            {

                if (!await _authService.checkHeader(roles, tokenHeader))
                {
                    return BadRequest("User not autenticated");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

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
    }
}