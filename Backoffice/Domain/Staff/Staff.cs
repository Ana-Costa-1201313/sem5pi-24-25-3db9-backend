using Backoffice.Domain.Shared;

namespace Backoffice.Domain.Staff
{
    public class Staff : Entity<StaffId>, IAggregateRoot
    {
        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string FullName { get; private set; }

        public int LicenseNumber { get; private set; }

        public Email Email { get; private set; }

        public PhoneNumber Phone { get; private set; }

        public string Specialization { get; private set; }

        public int AvailabilitySlots { get; private set; }

        public Role Role { get; private set; }

        public int RecruitmentYear { get; private set; }

        public int MecNumSequence { get; private set; }

        public MechanographicNumber MechanographicNum { get; private set; }

        private Staff()
        {

        }

        public Staff(CreateStaffDto dto, int mecNumSeq)
        {
            this.Id = new StaffId(Guid.NewGuid());
            this.FirstName = dto.FirstName;
            this.LastName = dto.LastName;
            this.FullName = dto.FullName;
            this.LicenseNumber = dto.LicenseNumber;
            this.Phone = new PhoneNumber(dto.Phone);
            this.Specialization = dto.Specialization;
            this.AvailabilitySlots = dto.AvailabilitySlots;
            this.Role = dto.Role;
            this.RecruitmentYear = dto.RecruitmentYear;
            this.MecNumSequence = mecNumSeq;
            this.MechanographicNum = new MechanographicNumber(dto.Role.ToString(), dto.RecruitmentYear, MecNumSequence);
            this.Email = new Email(MechanographicNum + "@healthcareapp.com");
        }
    }
}