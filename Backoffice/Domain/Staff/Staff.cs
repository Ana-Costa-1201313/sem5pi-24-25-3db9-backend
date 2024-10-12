namespace Backoffice.Domain.Staff
{
    public class Staff
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string FullName { get; private set; }
        public int LicenseNumber { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public string Specialization { get; private set; }
        public int AvailabilitySlots { get; private set; }
        public string StaffId { get; private set; }

        private Staff()
        {

        }

        public Staff(string firstName,
        string lastName,
        string fullName,
        int licenseNum,
        string email,
        string phone,
        string specialization,
        int availabilitySlots)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.FullName = fullName;
            this.LicenseNumber = licenseNum;
            this.Email = email;
            this.Phone = phone;
            this.Specialization = specialization;
            this.AvailabilitySlots = availabilitySlots;
        }
    }
}