using Backoffice.Domain.Appointments;
using Backoffice.Domain.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Backoffice.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly AppointmentService _service;
        private readonly AuthService _authService;
        public AppointmentController(AppointmentService service, AuthService authService)
        {
            _service = service;
            _authService = authService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAll()
        {/*
            try
            {
                await _authService.IsAuthorized(Request, new List<string> { "Admin" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }*/
            var appointments = await _service.GetAllAsync();

            if (appointments == null || appointments.Count == 0)
            {
                return NoContent();
            }

            return Ok(appointments);
        }

        [HttpPost]
        public async Task<ActionResult<AppointmentDto>> Create(CreateAppointmentDto appointmentDto)
        {/*
            try
            {
                await _authService.IsAuthorized(Request, new List<string> { "Admin" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }*/

            try
            {
                var appointment = await _service.CreateAsync(appointmentDto);
                return Ok(appointment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            //return null;
        }
    }
}