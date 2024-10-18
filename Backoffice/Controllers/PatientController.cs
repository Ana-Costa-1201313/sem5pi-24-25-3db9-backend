using Backoffice.Domain.Patients;
using Microsoft.AspNetCore.Mvc;

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
    }
}