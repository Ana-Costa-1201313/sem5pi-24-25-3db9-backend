using Backoffice.Domain.Shared;

namespace Backoffice.Domain.Staffs
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

        public List<AvailabilitySlot> AvailabilitySlots { get; private set; }

        public Role Role { get; private set; }

        public int MecNumSequence { get; private set; }

        public MechanographicNumber MechanographicNum { get; private set; }

        public bool Active { get; private set; }

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

            if (dto.Phone == null)
            {
                throw new BusinessRuleValidationException("Error: The staff must have a phone number!");
            }
            this.Phone = new PhoneNumber(dto.Phone);

            this.Specialization = dto.Specialization;
            this.AvailabilitySlots = new List<AvailabilitySlot>();

            foreach (var slotString in dto.AvailabilitySlots)
            {
                AvailabilitySlots.Add(AvailabilitySlot.CreateAvailabilitySlot(slotString));
            }

            if (dto.Role != Role.Admin && dto.Role != Role.Doctor && dto.Role != Role.Nurse && dto.Role != Role.Technician)
            {
                throw new BusinessRuleValidationException("Error: The staff role must be one of the following: Admin, Doctor, Nurse or Tech!");
            }
            this.Role = dto.Role;

            this.MecNumSequence = mecNumSeq;
            this.MechanographicNum = new MechanographicNumber(dto.Role.ToString(), dto.RecruitmentYear, MecNumSequence);
            this.Email = new Email(MechanographicNum + "@healthcareapp.com");
            this.Active = true;
        }

        public void Deactivate()
        {
            if (!this.Active)
            {
                throw new BusinessRuleValidationException("Error: This Staff profile is already deactivated!");
            }

            this.FirstName = "Deactivated Staff";
            this.LastName = "Deactivated Staff";
            this.FullName = "Deactivated Staff";
            this.LicenseNumber = this.Id.GetHashCode();
            this.Phone = null;
            this.Specialization = "Deactivated Staff";
            this.AvailabilitySlots = null;
            this.Active = false;
        }

        public void Edit(EditStaffDto dto)
        {
            if (!this.Active)
            {
                throw new BusinessRuleValidationException("Error: Can't update an inactive staff!");
            }

            if (dto.Phone == null)
            {
                throw new BusinessRuleValidationException("Error: The staff must have a phone number!");
            }

            this.Phone = new PhoneNumber(dto.Phone);
            this.Specialization = dto.Specialization;

            List<AvailabilitySlot> list = new List<AvailabilitySlot>();

            foreach (string s in dto.AvailabilitySlots)
            {
                list.Add(AvailabilitySlot.CreateAvailabilitySlot(s));
            }

            this.AvailabilitySlots = list;
        }

        public void PartialEdit(EditStaffDto dto)
        {
            if (!this.Active)
            {
                throw new BusinessRuleValidationException("Error: Can't update an inactive staff!");
            }

            if (dto.Phone != null)
            {
                this.Phone = new PhoneNumber(dto.Phone);
            }

            if (dto.Specialization != null)
            {
                this.Specialization = dto.Specialization;
            }

            if (dto.AvailabilitySlots != null)
            {
                List<AvailabilitySlot> list = new List<AvailabilitySlot>();

                foreach (string s in dto.AvailabilitySlots)
                {
                    list.Add(AvailabilitySlot.CreateAvailabilitySlot(s));
                }

                this.AvailabilitySlots = list;
            }
        }
    }
}