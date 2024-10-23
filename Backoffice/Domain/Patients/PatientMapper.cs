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
    public Patient ToPatient(CreatePatientDto dto,string medicalRecordNumber){
        return new Patient(dto,medicalRecordNumber);
    }

    public SearchPatientDto ToSearchPatientDto(Patient patient){
        return new SearchPatientDto{
            FullName = patient.FullName,
            Email = patient.Email._Email,
            DateOfBirth = patient.DateOfBirth
        };
    }
}