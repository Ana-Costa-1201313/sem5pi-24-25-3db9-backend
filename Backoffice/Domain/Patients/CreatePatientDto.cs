namespace Backoffice.Domain.Patients
{
    public class PatientDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Email Email { get; set; }
        public PhoneNumber Phone { get; set; }
        public string[] Allergies { get; set; }
        public int MedicalRecordNumber { get; set; }
    }
}