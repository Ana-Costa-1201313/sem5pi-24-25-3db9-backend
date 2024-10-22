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
        public async Task<List<Patient>> GetPatientsByNameAsync(string name)
        {
        return await _context.Patients
            .Where(p => p.FullName.Contains(name)) 
            .ToListAsync();
        }
        

        public async Task<List<Patient>> GetPatientsByDateOfBirth(DateTime dateOfBirth)
        {
            return await _context.Patients
            .Where(p => p.DateOfBirth == dateOfBirth)
            .ToListAsync();
        }

        public async Task<List<Patient>> GetPatientsByMedicalRecordNumber(int medicalRecordNumber){
            return await _context.Patients
            .Where(p=> p.MedicalRecordNumber == medicalRecordNumber)
            .ToListAsync();
        }
        }
}