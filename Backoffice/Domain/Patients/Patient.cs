using System.Security.Cryptography;
using Backoffice.Domain.Shared;

namespace Backoffice.Domain.Patients
{
    public class Patient : Entity<PatientId>, IAggregateRoot
    {
        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string FullName { get; private set; }

        public string Gender { get; private set; }

        public DateTime DateOfBirth { get; private set; }

        public Email Email { get; private set; }

        public PhoneNumber Phone { get; private set; }

        public string[] Allergies { get; private set; }

        public int MedicalRecordNumber { get; private set; }

        //public Appointment[] AppointmentHistory { get; private set; }

        public string PatientId { get; private set; }

        private Patient(){}

        public Patient(CreatePatientDto dto)
        {
            this.Id = new PatientId(Guid.NewGuid());
            this.FirstName = dto.FirstName;
            this.LastName = dto.LastName;
            this.FullName = dto.FullName;
            this.LicenseNumber = dto.LicenseNumber;
            this.Email = new Email(dto.Email);
            this.Phone = new PhoneNumber(dto.Phone);
            this.Specialization = dto.Specialization;
            this.AvailabilitySlots = dto.AvailabilitySlots;
        }
    }
}