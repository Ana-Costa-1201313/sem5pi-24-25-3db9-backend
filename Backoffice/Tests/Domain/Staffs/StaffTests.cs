using Backoffice.Domain.Shared;
using Backoffice.Domain.Staffs;
using Xunit;

namespace Backoffice.Tests
{
    public class StaffTests
    {
        [Fact]
        public void ValidStaff()
        {
            List<string> AvailabilitySlots = new List<string>();
            AvailabilitySlots.Add("2024 - 10 - 10T12: 00:00 / 2024 - 10 - 11T15: 00:00");
            AvailabilitySlots.Add("2024 - 10 - 14T12: 00:00 / 2024 - 10 - 19T15: 00:00");

            CreateStaffDto dto = new CreateStaffDto
            {
                FirstName = "ana",
                LastName = "costa",
                FullName = "ana costa",
                LicenseNumber = 1,
                Phone = "999999999",
                Specialization = "spec",
                AvailabilitySlots = AvailabilitySlots,
                Role = Role.Nurse,
                RecruitmentYear = 2024
            };


            string sFirstName = "ana";
            string sLastName = "costa";
            string sFullName = "ana costa";
            int sLicenseNumber = 1;
            PhoneNumber sPhone = new PhoneNumber("999999999");
            string sSpecialization = "spec";
            Role sRole = Role.Nurse;
            int MecNumSequence = 1;
            MechanographicNumber MechanographicNum = new MechanographicNumber(sRole.ToString(), dto.RecruitmentYear, MecNumSequence);
            Email Email = new Email(MechanographicNum + "@healthcareapp.com");


            var staff = new Staff(dto, MecNumSequence);


            Assert.NotNull(staff.Id);
            Assert.Equal(sFirstName, staff.FirstName);
            Assert.Equal(sLastName, staff.LastName);
            Assert.Equal(sFullName, staff.FullName);
            Assert.Equal(sLicenseNumber, staff.LicenseNumber.LicenseNum);
            Assert.Equal(sPhone.PhoneNum, staff.Phone.PhoneNum);
            Assert.Equal(sSpecialization, staff.Specialization);
            Assert.Equal(sRole, staff.Role);
            Assert.Equal(MecNumSequence, staff.MecNumSequence);
            Assert.Equal(MechanographicNum.MechanographicNum, staff.MechanographicNum.MechanographicNum);
            Assert.Equal(Email._Email, staff.Email._Email);
        }


        [Fact]
        public void NullFirstName()
        {
            List<string> AvailabilitySlots = new List<string>();
            AvailabilitySlots.Add("2024 - 10 - 10T12: 00:00 / 2024 - 10 - 11T15: 00:00");
            AvailabilitySlots.Add("2024 - 10 - 14T12: 00:00 / 2024 - 10 - 19T15: 00:00");

            CreateStaffDto dto = new CreateStaffDto
            {
                FirstName = null,
                LastName = "costa",
                FullName = "ana costa",
                LicenseNumber = 1,
                Phone = "999999999",
                Specialization = "spec",
                AvailabilitySlots = AvailabilitySlots,
                Role = Role.Nurse,
                RecruitmentYear = 2024
            };


            int MecNumSequence = 1;

            var exception = Assert.Throws<BusinessRuleValidationException>(() => new Staff(dto, MecNumSequence));
            Assert.Equal("Error: The staff must have a first name!", exception.Message);
        }

        [Fact]
        public void EmptyFirstName()
        {
            List<string> AvailabilitySlots = new List<string>();
            AvailabilitySlots.Add("2024 - 10 - 10T12: 00:00 / 2024 - 10 - 11T15: 00:00");
            AvailabilitySlots.Add("2024 - 10 - 14T12: 00:00 / 2024 - 10 - 19T15: 00:00");

            CreateStaffDto dto = new CreateStaffDto
            {
                FirstName = "",
                LastName = "costa",
                FullName = "ana costa",
                LicenseNumber = 1,
                Phone = "999999999",
                Specialization = "spec",
                AvailabilitySlots = AvailabilitySlots,
                Role = Role.Nurse,
                RecruitmentYear = 2024
            };


            int MecNumSequence = 1;

            var exception = Assert.Throws<BusinessRuleValidationException>(() => new Staff(dto, MecNumSequence));
            Assert.Equal("Error: The staff must have a first name!", exception.Message);
        }

        [Fact]
        public void NullLastName()
        {
            List<string> AvailabilitySlots = new List<string>();
            AvailabilitySlots.Add("2024 - 10 - 10T12: 00:00 / 2024 - 10 - 11T15: 00:00");
            AvailabilitySlots.Add("2024 - 10 - 14T12: 00:00 / 2024 - 10 - 19T15: 00:00");

            CreateStaffDto dto = new CreateStaffDto
            {
                FirstName = "ana",
                LastName = null,
                FullName = "ana costa",
                LicenseNumber = 1,
                Phone = "999999999",
                Specialization = "spec",
                AvailabilitySlots = AvailabilitySlots,
                Role = Role.Nurse,
                RecruitmentYear = 2024
            };


            int MecNumSequence = 1;

            var exception = Assert.Throws<BusinessRuleValidationException>(() => new Staff(dto, MecNumSequence));
            Assert.Equal("Error: The staff must have a last name!", exception.Message);
        }

        [Fact]
        public void EmptyLastName()
        {
            List<string> AvailabilitySlots = new List<string>();
            AvailabilitySlots.Add("2024 - 10 - 10T12: 00:00 / 2024 - 10 - 11T15: 00:00");
            AvailabilitySlots.Add("2024 - 10 - 14T12: 00:00 / 2024 - 10 - 19T15: 00:00");

            CreateStaffDto dto = new CreateStaffDto
            {
                FirstName = "ana",
                LastName = "",
                FullName = "ana costa",
                LicenseNumber = 1,
                Phone = "999999999",
                Specialization = "spec",
                AvailabilitySlots = AvailabilitySlots,
                Role = Role.Nurse,
                RecruitmentYear = 2024
            };


            int MecNumSequence = 1;

            var exception = Assert.Throws<BusinessRuleValidationException>(() => new Staff(dto, MecNumSequence));
            Assert.Equal("Error: The staff must have a last name!", exception.Message);
        }

        [Fact]
        public void NullFullName()
        {
            List<string> AvailabilitySlots = new List<string>();
            AvailabilitySlots.Add("2024 - 10 - 10T12: 00:00 / 2024 - 10 - 11T15: 00:00");
            AvailabilitySlots.Add("2024 - 10 - 14T12: 00:00 / 2024 - 10 - 19T15: 00:00");

            CreateStaffDto dto = new CreateStaffDto
            {
                FirstName = "ana",
                LastName = "costa",
                FullName = null,
                LicenseNumber = 1,
                Phone = "999999999",
                Specialization = "spec",
                AvailabilitySlots = AvailabilitySlots,
                Role = Role.Nurse,
                RecruitmentYear = 2024
            };


            int MecNumSequence = 1;

            var exception = Assert.Throws<BusinessRuleValidationException>(() => new Staff(dto, MecNumSequence));
            Assert.Equal("Error: The staff must have a full name!", exception.Message);
        }

        [Fact]
        public void EmptyFullName()
        {
            List<string> AvailabilitySlots = new List<string>();
            AvailabilitySlots.Add("2024 - 10 - 10T12: 00:00 / 2024 - 10 - 11T15: 00:00");
            AvailabilitySlots.Add("2024 - 10 - 14T12: 00:00 / 2024 - 10 - 19T15: 00:00");

            CreateStaffDto dto = new CreateStaffDto
            {
                FirstName = "ana",
                LastName = "costa",
                FullName = "",
                LicenseNumber = 1,
                Phone = "999999999",
                Specialization = "spec",
                AvailabilitySlots = AvailabilitySlots,
                Role = Role.Nurse,
                RecruitmentYear = 2024
            };


            int MecNumSequence = 1;

            var exception = Assert.Throws<BusinessRuleValidationException>(() => new Staff(dto, MecNumSequence));
            Assert.Equal("Error: The staff must have a full name!", exception.Message);
        }

        [Fact]
        public void SmallPhone()
        {
            List<string> AvailabilitySlots = new List<string>();
            AvailabilitySlots.Add("2024 - 10 - 10T12: 00:00 / 2024 - 10 - 11T15: 00:00");
            AvailabilitySlots.Add("2024 - 10 - 14T12: 00:00 / 2024 - 10 - 19T15: 00:00");

            CreateStaffDto dto = new CreateStaffDto
            {
                FirstName = "ana",
                LastName = "costa",
                FullName = "ana costa",
                LicenseNumber = 1,
                Phone = "99999",
                Specialization = "spec",
                AvailabilitySlots = AvailabilitySlots,
                Role = Role.Nurse,
                RecruitmentYear = 2024
            };


            int MecNumSequence = 1;

            var exception = Assert.Throws<BusinessRuleValidationException>(() => new Staff(dto, MecNumSequence));
            Assert.Equal("Error: The phone number is invalid!", exception.Message);
        }

        [Fact]
        public void InvalidPhone()
        {
            List<string> AvailabilitySlots = new List<string>();
            AvailabilitySlots.Add("2024 - 10 - 10T12: 00:00 / 2024 - 10 - 11T15: 00:00");
            AvailabilitySlots.Add("2024 - 10 - 14T12: 00:00 / 2024 - 10 - 19T15: 00:00");

            CreateStaffDto dto = new CreateStaffDto
            {
                FirstName = "ana",
                LastName = "costa",
                FullName = "ana costa",
                LicenseNumber = 1,
                Phone = "199999999",
                Specialization = "spec",
                AvailabilitySlots = AvailabilitySlots,
                Role = Role.Nurse,
                RecruitmentYear = 2024
            };


            int MecNumSequence = 1;

            var exception = Assert.Throws<BusinessRuleValidationException>(() => new Staff(dto, MecNumSequence));
            Assert.Equal("Error: The phone number is invalid!", exception.Message);
        }

        [Fact]
        public void WrongRole()
        {
            List<string> AvailabilitySlots = new List<string>();
            AvailabilitySlots.Add("2024 - 10 - 10T12: 00:00 / 2024 - 10 - 11T15: 00:00");
            AvailabilitySlots.Add("2024 - 10 - 14T12: 00:00 / 2024 - 10 - 19T15: 00:00");

            CreateStaffDto dto = new CreateStaffDto
            {
                FirstName = "ana",
                LastName = "costa",
                FullName = "ana costa",
                LicenseNumber = 1,
                Phone = "999999999",
                Specialization = "spec",
                AvailabilitySlots = AvailabilitySlots,
                Role = Role.Patient,
                RecruitmentYear = 2024
            };


            int MecNumSequence = 1;

            var exception = Assert.Throws<BusinessRuleValidationException>(() => new Staff(dto, MecNumSequence));
            Assert.Equal("Error: The staff role must be one of the following: Admin, Doctor, Nurse or Tech!", exception.Message);
        }



        [Fact]
        public void InvalidSlotFormat()
        {
            List<string> AvailabilitySlots = new List<string>();
            AvailabilitySlots.Add("2024 - 10 - 10T12: 00:00 - 2024 - 10 - 11T15: 00:00");

            CreateStaffDto dto = new CreateStaffDto
            {
                FirstName = "ana",
                LastName = "costa",
                FullName = "ana costa",
                LicenseNumber = 1,
                Phone = "999999999",
                Specialization = "spec",
                AvailabilitySlots = AvailabilitySlots,
                Role = Role.Nurse,
                RecruitmentYear = 2024
            };


            int MecNumSequence = 1;

            var exception = Assert.Throws<BusinessRuleValidationException>(() => new Staff(dto, MecNumSequence));
            Assert.Equal("Error: Invalid Availability slot format!", exception.Message);
        }

        [Fact]
        public void InvalidMecNumSeq()
        {
            List<string> AvailabilitySlots = new List<string>();
            AvailabilitySlots.Add("2024 - 10 - 10T12: 00:00 / 2024 - 10 - 11T15: 00:00");
            AvailabilitySlots.Add("2024 - 10 - 14T12: 00:00 / 2024 - 10 - 19T15: 00:00");

            CreateStaffDto dto = new CreateStaffDto
            {
                FirstName = "ana",
                LastName = "costa",
                FullName = "ana costa",
                LicenseNumber = 1,
                Phone = "999999999",
                Specialization = "spec",
                AvailabilitySlots = AvailabilitySlots,
                Role = Role.Nurse,
                RecruitmentYear = 2024
            };


            int MecNumSequence = -1;

            var exception = Assert.Throws<BusinessRuleValidationException>(() => new Staff(dto, MecNumSequence));
            Assert.Equal("Error: The mechanographic number must be bigger than zero!", exception.Message);
        }

        [Fact]
        public void InvalidRecYear()
        {
            List<string> AvailabilitySlots = new List<string>();
            AvailabilitySlots.Add("2024 - 10 - 10T12: 00:00 / 2024 - 10 - 11T15: 00:00");
            AvailabilitySlots.Add("2024 - 10 - 14T12: 00:00 / 2024 - 10 - 19T15: 00:00");

            CreateStaffDto dto = new CreateStaffDto
            {
                FirstName = "ana",
                LastName = "costa",
                FullName = "ana costa",
                LicenseNumber = 1,
                Phone = "999999999",
                Specialization = "spec",
                AvailabilitySlots = AvailabilitySlots,
                Role = Role.Nurse,
                RecruitmentYear = -2024
            };


            int MecNumSequence = 1;

            var exception = Assert.Throws<BusinessRuleValidationException>(() => new Staff(dto, MecNumSequence));
            Assert.Equal("Error: The year must be bigger than zero!", exception.Message);
        }

    }
}