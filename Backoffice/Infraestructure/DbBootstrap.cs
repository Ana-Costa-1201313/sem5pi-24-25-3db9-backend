using System;
using System.Linq;
using Backoffice.Infraestructure;
using Backoffice.Domain.Specializations;
using Microsoft.Extensions.DependencyInjection;
using Backoffice.Domain.Staffs;
using Backoffice.Domain.Shared;
using Backoffice.Domain.Users;

namespace Backoffice.Infraestructure
{
    public class DbBootstrap
    {
        private readonly BDContext _context;

        public DbBootstrap(BDContext context)
        {
            _context = context;
        }

        public void UserBootstrap()
        {
            if (_context.Users.Any()) return;

            List<User> users = new List<User>();

            User user1 = new User(Role.Admin, "admin@hotmail.com");
            user1.ActivateUser("AAAAAAAAAAA1!");

            User user2 = new User(Role.Nurse, "N20241@healthcareapp.com");
            user2.ActivateUser("AAAAAAAAAAA1!");

            User user3 = new User(Role.Doctor, "D20232@healthcareapp.com");
            user3.ActivateUser("AAAAAAAAAAA1!");

            User user4 = new User(Role.Doctor, "D20243@healthcareapp.com");
            user4.ActivateUser("AAAAAAAAAAA1!");

            User user5 = new User(Role.Nurse, "N20224@healthcareapp.com");
            user5.ActivateUser("AAAAAAAAAAA1!");

            User user6 = new User(Role.Technician, "T20225@healthcareapp.com");
            user6.ActivateUser("AAAAAAAAAAA1!");

            User user7 = new User(Role.Nurse, "N20236@healthcareapp.com");
            user7.ActivateUser("AAAAAAAAAAA1!");

            users.Add(user1);
            users.Add(user2);
            users.Add(user3);
            users.Add(user3);
            users.Add(user4);
            users.Add(user5);
            users.Add(user6);
            users.Add(user7);

            _context.Users.AddRange(users);
            _context.SaveChanges();
        }

        public void SpecializationBootstrap()
        {
            if (_context.Specializations.Any()) return;

            var specializations = new[]
            {
                new Specialization(new SpecializationName("Cardiology")),
                new Specialization(new SpecializationName("Dermatology")),
                new Specialization(new SpecializationName("Ophthalmology")),
                new Specialization(new SpecializationName("Orthopedics"))
            };
            _context.Specializations.AddRange(specializations);
            _context.SaveChanges();
        }

        public void StaffBootstrap()
        {
            if (_context.Staff.Any()) return;

            var cardiology = _context.Specializations.FirstOrDefault(s => s.Name.Name == "Cardiology");
            var dermatology = _context.Specializations.FirstOrDefault(s => s.Name.Name == "Dermatology");
            var ophthalmology = _context.Specializations.FirstOrDefault(s => s.Name.Name == "Ophthalmology");
            var orthopedics = _context.Specializations.FirstOrDefault(s => s.Name.Name == "Orthopedics");

            if (cardiology == null || dermatology == null || ophthalmology == null || orthopedics == null)
            {
                throw new InvalidOperationException("The specializations must be created before the staff");
            }

            List<string> AvailabilitySlots = new List<string>();
            AvailabilitySlots.Add("2024 - 10 - 10T12: 00:00 / 2024 - 10 - 11T15: 00:00");
            AvailabilitySlots.Add("2024 - 10 - 14T12: 00:00 / 2024 - 10 - 19T15: 00:00");

            CreateStaffDto dto1 = new CreateStaffDto
            {
                Name = "Ana Costa",
                LicenseNumber = 13557,
                Phone = "929234899",
                Specialization = "Cardiology",
                AvailabilitySlots = AvailabilitySlots,
                Role = Role.Nurse,
                RecruitmentYear = 2024
            };

            CreateStaffDto dto2 = new CreateStaffDto
            {
                Name = "Maria Silva",
                LicenseNumber = 28234,
                Phone = "929244899",
                Specialization = "Cardiology",
                AvailabilitySlots = AvailabilitySlots,
                Role = Role.Doctor,
                RecruitmentYear = 2023
            };

            CreateStaffDto dto3 = new CreateStaffDto
            {
                Name = "Mário Ferreira",
                LicenseNumber = 39324,
                Phone = "929244299",
                Specialization = "Dermatology",
                AvailabilitySlots = AvailabilitySlots,
                Role = Role.Doctor,
                RecruitmentYear = 2024
            };

            CreateStaffDto dto4 = new CreateStaffDto
            {
                Name = "Pedro Campos",
                LicenseNumber = 45173,
                Phone = "929244259",
                Specialization = "Dermatology",
                AvailabilitySlots = AvailabilitySlots,
                Role = Role.Nurse,
                RecruitmentYear = 2022
            };

            CreateStaffDto dto5 = new CreateStaffDto
            {
                Name = "Rita Sousa",
                LicenseNumber = 45613,
                Phone = "929244339",
                Specialization = null,
                AvailabilitySlots = AvailabilitySlots,
                Role = Role.Technician,
                RecruitmentYear = 2022
            };

            CreateStaffDto dto6 = new CreateStaffDto
            {
                Name = "Sofia Ferreira",
                LicenseNumber = 45123,
                Phone = "929245339",
                Specialization = null,
                AvailabilitySlots = AvailabilitySlots,
                Role = Role.Nurse,
                RecruitmentYear = 2023
            };

             CreateStaffDto dto7 = new CreateStaffDto
            {
                Name = "José Matos",
                LicenseNumber = 25113,
                Phone = "923245369",
                Specialization = "Dermatology",
                AvailabilitySlots = AvailabilitySlots,
                Role = Role.Nurse,
                RecruitmentYear = 2024
            };

            List<Staff> staffList = new List<Staff>();

            Staff staff1 = new Staff(dto1, cardiology, 1, "healthcareapp.com");
            Staff staff2 = new Staff(dto2, cardiology, 2, "healthcareapp.com");
            Staff staff3 = new Staff(dto3, ophthalmology, 3, "healthcareapp.com");
            Staff staff4 = new Staff(dto4, orthopedics, 4, "healthcareapp.com");
            Staff staff5 = new Staff(dto5, null, 5, "healthcareapp.com");
            Staff staff6 = new Staff(dto6, ophthalmology, 6, "healthcareapp.com");
            Staff staff7 = new Staff(dto7, dermatology, 7, "healthcareapp.com");

            staff6.Deactivate();

            staffList.Add(staff1);
            staffList.Add(staff2);
            staffList.Add(staff3);
            staffList.Add(staff4);
            staffList.Add(staff5);
            staffList.Add(staff6);
            staffList.Add(staff7);

            _context.Staff.AddRange(staffList);
            _context.SaveChanges();
        }
    }
}
