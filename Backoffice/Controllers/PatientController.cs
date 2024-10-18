using Backoffice.Domain.Patient;
using Backoffice.Domain.Patients;
using Backoffice.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

namespace Backoffice.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly PatientService _service;

        public PatientController(PatientService service)
        {
            _service = service;
        }

        //Obter todos os patients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientDto>>> GetAll()
        {
            return await _service.GetAllAsync();  
        }

        //Obter patient por Id
        [HttpGet("{id}")]
        public async Task<ActionResult<PatientDto>> GetById(Guid id)
        {
            var patient = await _service.GetByIdAsync(id);

            if (patient == null)
            {
                return NotFound();
            }

            return patient;
        }
        [HttpPost]
        public async Task<ActionResult<PatientDto>> Create(CreatePatientDto dto)
        {
            try {
                var patient = await _service.AddAsync(dto);
                return CreatedAtAction(nameof(GetById),new {id = patient.Id},patient);
            
            } catch(BusinessRuleValidationException e) {
                    return BadRequest(new {Message = e.Message});
            }
        }
    }
}