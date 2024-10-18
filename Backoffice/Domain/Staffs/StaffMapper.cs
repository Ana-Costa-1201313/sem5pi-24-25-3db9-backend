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
                FirstName = staff.FirstName,
                LastName = staff.LastName,
                FullName = staff.FullName,
                LicenseNumber = staff.LicenseNumber,
                Email = staff.Email._Email,
                Phone = staff.Phone.PhoneNum,
                Specialization = staff.Specialization,
                AvailabilitySlots = stringAvailabilitySlots,
                Role = staff.Role,
                MechanographicNum = staff.MechanographicNum.ToString(),
                Active = staff.Active
            };
        }

        public Staff ToStaff(CreateStaffDto dto, int mecNumSeq) {
            return new Staff(dto, mecNumSeq);
        }

        
    }
}