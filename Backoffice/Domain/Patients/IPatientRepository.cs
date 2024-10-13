using Backoffice.Domain.Shared;

namespace Backoffice.Domain.Patients
{
    public interface IPatientRepository : IRepository<Patient, PatientId>
    {

    }
}