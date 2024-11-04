using Backoffice.Domain.Appointments;
using Backoffice.Infraestructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace Backoffice.Infraestructure.Appointments
{
    public class AppointmentRepository : BaseRepository<Appointment, AppointmentId>, IAppointmentRepository
    {
        private readonly BDContext _context;
        public AppointmentRepository(BDContext context) : base(context.Appointments)
        {
            _context = context;
        }

        public async Task<List<Appointment>> GetAllAppointmentsAsync()
        {
            return await _context.Appointments
                .ToListAsync()
            ;
        }
    }
}