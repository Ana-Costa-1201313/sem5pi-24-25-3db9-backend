namespace Backoffice.Domain.Staffs
{
    public class StaffMapper
    {
        public StaffDto ToStaffDto(Staff staff)
        {
            var stringAvailabilitySlots = new List<string>();

            foreach (var availabilitySlot in staff.AvailabilitySlots)
            {
                stringAvailabilitySlots.Add(availabilitySlot.ToString());
            }

            return new StaffDto
            {
                Id = staff.Id.AsGuid(),
                Name = staff.Name,
                LicenseNumber = staff.LicenseNumber,
                Email = staff.Email._Email,
                Phone = staff.Phone.PhoneNum,
                Specialization = staff.Specialization,
                AvailabilitySlots = stringAvailabilitySlots,
                Role = staff.Role,
                MechanographicNum = staff.MechanographicNum.ToString()
            };
        }

        public Staff ToStaff(CreateStaffDto dto, int mecNumSeq, string dns) {
            return new Staff(dto, mecNumSeq, dns);
        }

        
    }
}