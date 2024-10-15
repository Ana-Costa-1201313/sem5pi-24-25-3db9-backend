using Backoffice.Domain.Shared;
using Backoffice.Domain.Users;

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

        public int MechanographicNum { get; private set; }

        private Staff()
        {

        }

        public Staff(CreateStaffDto dto, int mecNum)
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
            this.MechanographicNum = mecNum;
            this.Email = new Email(Role.ToString().Substring(0, 1) + MechanographicNum + "@healthcareapp.com");

        }
    }
}