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

        public string StaffId { get; private set; }

        private Staff()
        {

        }

        public Staff(CreateStaffDto dto)
        {
            this.Id = new StaffId(Guid.NewGuid());
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