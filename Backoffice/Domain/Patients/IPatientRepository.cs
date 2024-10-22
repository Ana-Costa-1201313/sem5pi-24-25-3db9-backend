using Backoffice.Domain.Shared;

namespace Backoffice.Domain.Patients
{
    public interface IPatientRepository : IRepository<Patient, PatientId>
    {
        Task<Patient> GetPatientByEmailAsync(Email email);
        Task<List<Patient>> GetPatientsByNameAsync(string name);
        Task<List<Patient>> GetPatientsByDateOfBirth(DateTime dateOfBirth);
        Task<List<Patient>> GetPatientsByMedicalRecordNumber(int medicalRecordNumber);
        Task<List<Patient>> SearchPatientsAsync(string name, string email,DateTime? dateOfBirth
        , int? medicalRecordNumber);
    }
}