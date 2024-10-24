using Backoffice.Domain.Shared;
using Backoffice.Domain.Staffs;
using Microsoft.Extensions.Primitives;
using System.Net.Mail;

namespace Backoffice.Domain.Users
{
    public class UserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _repo;
        private readonly IStaffRepository _staffRepo;


        private readonly IEmailService _emailService;
        private readonly ExternalApiServices _externalApiServices;

        // passar isto para o configurations file
        private readonly string emailBody = $"https://localhost:5001/api/Users/";

        public UserService(IUnitOfWork unitOfWork, IUserRepository repo, IEmailService emailService, ExternalApiServices externalApiServices, IStaffRepository staffRepo)
        {
            this._unitOfWork = unitOfWork;
            this._repo = repo;
            this._emailService = emailService;
            this._externalApiServices = externalApiServices;
            this._staffRepo = staffRepo;
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            var list = await this._repo.GetAllAsync();

            List<UserDto> listDto = list.ConvertAll<UserDto>(u => new UserDto
            {
                Id = u.Id.AsGuid(),
                Role = u.Role,
                Email = u.Email.ToString(),
                Password = u.Password
            });

            return listDto;
        }

        public async Task<UserDto> GetByIdAsync(Guid id)
        {
            var user = await this._repo.GetByIdAsync(new UserId(id));

            if (user == null)
                return null;

            return new UserDto
            {
                Id = user.Id.AsGuid(),
                Role = user.Role,
                Email = user.Email.ToString(),
                Password = user.Password
            };
        }

        public async Task<UserDto> AddAsync(CreateUserDto dto)
        {
            Staff staff = await _staffRepo.GetStaffByEmailAsync(new Email(dto.Email));

            if (dto.Role == Role.Patient || dto.Role == Role.Admin)

            {
                if (staff != null)
                {
                    throw new BusinessRuleValidationException("Error: That email is being used by a staff member!");
                }
            }
            else
            {
                if (staff == null || staff.Role != dto.Role)
                {
                    throw new BusinessRuleValidationException("Error: The user role doesn't match the email!");
                }
            }

            User user = new User(dto.Role, dto.Email);


            await this._repo.AddAsync(user);

            await this._unitOfWork.CommitAsync();

            string messageBodyParameters = $"{user.Id.AsGuid()}?password=changeMeToYourNewPassword";
            string messageSubject = "change your password";
            try
            {
                await _emailService.SendEmail(dto.Email, emailBody + messageBodyParameters, messageSubject);
            }
            catch (SmtpException ex) {
                Console.WriteLine(ex.Message);
            }
            return new UserDto
            {
                Id = user.Id.AsGuid(),
                Role = user.Role,
                Email = user.Email.ToString(),
                Password = user.Password,
                Active = user.Active

            };
        }

        public async Task<UserDto> UpdatePassword(Guid id, string password)
        {

            //pasword should be hashed for security

            var user = await this._repo.GetByIdAsync(new UserId(id));

            if (user == null)
            {
                return null;
            }

            user.ActivateUser(password);

            await this._unitOfWork.CommitAsync();

            return new UserDto
            {
                Id = user.Id.AsGuid(),
                Role = user.Role,
                Email = user.Email.ToString(),
                Password = user.Password,
                Active = user.Active
            };
        }


        public async void sendConfirmationEmail(UserDto user)
        {
            if (user == null)
            {
                throw new NullReferenceException("No user to send confirmation eail to!");
            }
            try
            {
                await _emailService.SendEmail(user.Email, $"New account has been created with password: {user.Password.ToString} .", "Confirmation email");
                //await _emailService.SendEmail("1221695@isep.ipp.pt", $"New account has been created with password: { user.Password.Passwd} .","Confirmation email");
            }
            catch (SmtpException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}