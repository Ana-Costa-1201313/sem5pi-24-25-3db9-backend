using Backoffice.Domain.Shared;

namespace Backoffice.Domain.Staffs
{
    public class Staff : Entity<StaffId>, IAggregateRoot
    {
        public string Name { get; private set; }

        public int LicenseNumber { get; private set; }

        public Email Email { get; private set; }

        public PhoneNumber Phone { get; private set; }

        public string Specialization { get; private set; }

        public List<AvailabilitySlot> AvailabilitySlots { get; private set; }

        public Role Role { get; private set; }

        public int MecNumSequence { get; private set; }

        public MechanographicNumber MechanographicNum { get; private set; }

        private Staff()
        {

        }

        public Staff(CreateStaffDto dto, int mecNumSeq, string dns)
        {
            this.Id = new StaffId(Guid.NewGuid());
            this.Name = dto.Name;
            this.LicenseNumber = dto.LicenseNumber;
            this.Phone = new PhoneNumber(dto.Phone);
            this.Specialization = dto.Specialization;
            this.AvailabilitySlots = new List<AvailabilitySlot>();

            foreach (var slotString in dto.AvailabilitySlots)
            {
                var times = slotString.Split('/');
                if (times.Length == 2)
                {
                    var startTime = times[0];
                    var endTime = times[1];

                    this.AvailabilitySlots.Add(new AvailabilitySlot(startTime, endTime));
                }
                else
                {
                    throw new BusinessRuleValidationException("Error: Invalid Availability slot format!");
                }
            }

            this.Role = dto.Role;
            this.MecNumSequence = mecNumSeq;
            this.MechanographicNum = new MechanographicNumber(dto.Role.ToString(), dto.RecruitmentYear, MecNumSequence);
            this.Email = new Email(MechanographicNum + "@" + dns);
        }
    }
}