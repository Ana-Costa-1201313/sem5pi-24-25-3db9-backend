using Backoffice.Domain.Patients;
using Backoffice.Infraestructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace Backoffice.Infraestructure.Patients
{
    public class PatientRepository : BaseRepository<Patient, PatientId>, IPatientRepository
    {
        private readonly DbContext _context;
        public PatientRepository(BDContext context) : base(context.Patients)
        {
            _context = context;
        }

        
    }
}