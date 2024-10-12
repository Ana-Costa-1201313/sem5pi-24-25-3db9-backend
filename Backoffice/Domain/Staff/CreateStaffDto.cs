namespace Backoffice.Domain.Staff
{
    public class CreateStaffDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public int LicenseNumber { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Specialization { get; set; }
        public int AvailabilitySlots { get; set; }
    }
}