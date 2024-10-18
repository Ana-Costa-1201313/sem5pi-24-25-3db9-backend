using Backoffice.Domain.Patient;

namespace Backoffice.Domain.Patients;

public class PatientMapper{

    public PatientDto ToPatientDto(Patient patient){
        return new PatientDto{
            Id = patient.Id.AsGuid(),
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            FullName = patient.FullName,
            Gender = patient.Gender,
            DateOfBirth = patient.DateOfBirth,
            Email = patient.Email._Email,
            Phone = patient.Phone.PhoneNum,
            Allergies = patient.Allergies,
            MedicalRecordNumber = patient.MedicalRecordNumber
        };
    }
    public Patient ToPatient(CreatePatientDto dto){
        return new Patient(dto);
    }
}