using Backoffice.Domain.Patients;
using Backoffice.Domain.Shared;
using Backoffice.Infraestructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace Backoffice.Infraestructure.Patients
{
    public class PatientRepository : BaseRepository<Patient, PatientId>, IPatientRepository
    {
        private readonly BDContext _context;
        public PatientRepository(BDContext context) : base(context.Patients)
        {
            _context = context;
        }
        public async Task<Patient> GetPatientByEmailAsync(Email email){
            return await _context.Patients.Where(p => p.Email == email).FirstOrDefaultAsync();
        }
    }
}