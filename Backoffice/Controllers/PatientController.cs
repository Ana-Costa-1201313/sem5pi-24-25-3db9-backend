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
        //Criar um patient profile
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
        //Dar update PUT: api/Patients
        [HttpPut("{id}")]
        public async Task<ActionResult<PatientDto>> Update(Guid id,EditPatientDto dto)
        {
           
            try{
                var updatedPatient = await _service.UpdateAsync(id,dto);

                if(updatedPatient == null){
                    return NotFound(); //Erro 404 caso o patient profile nao tenha sido encontrado
                }
                return Ok(updatedPatient); // Sucesso
            }
            catch (BusinessRuleValidationException ex) {
                    return BadRequest(new {Message = ex.Message}); // Erro 400 caso haja uma exceção na regra de negocio ou seja na alteração de algum dado
            }
        }
        //Dar Delete a um patient profile
        
    }
}