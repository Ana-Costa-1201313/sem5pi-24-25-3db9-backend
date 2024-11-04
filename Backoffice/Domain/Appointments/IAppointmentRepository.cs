using Backoffice.Domain.Shared;

namespace Backoffice.Domain.Appointments
{
    public interface IAppointmentRepository : IRepository<Appointment, AppointmentId>
    {
        public Task<List<Appointment>> GetAllAppointmentsAsync();
        //public Task<Appointment> GetOpRequestByIdAsync(AppointmentId id);
        //public Task<List<Appointment>> GetOpRequestsByDoctorIdAsync(StaffId doctorId);
    }
}