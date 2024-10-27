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

        public async Task<List<Patient>> SearchPatientsAsync(string name, string email,DateTime? dateOfBirth, string medicalRecordNumber)
        {
            var query = _context.Patients.AsQueryable();

            if(!string.IsNullOrEmpty(name))
                query = query.Where(p => p.FullName.Contains(name));
            if(!string.IsNullOrEmpty(email))
                query = query.Where(p => p.EmailAddress == email);
            if(dateOfBirth.HasValue)
                query = query.Where(p => p.DateOfBirth == dateOfBirth);
            if(!string.IsNullOrEmpty(medicalRecordNumber))
                query = query.Where(p=> p.MedicalRecordNumber == medicalRecordNumber);

                return await query.ToListAsync();
        }


        public async Task<Patient> GetPatientByEmailAsync(Email email){
            return await _context.Patients.Where(p => p.Email == email).FirstOrDefaultAsync();
        }
        
        public async Task<Patient> GetLatestPatientByMonthAsync()
        {
            return await _context.Patients.OrderByDescending(p=> p.MedicalRecordNumber).FirstOrDefaultAsync();
        
       
        }
    }
}