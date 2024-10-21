using Backoffice.Domain.Shared;
using Backoffice.Services;
using Microsoft.Extensions.Primitives;
using System.Net.Mail;

namespace Backoffice.Domain.Users
{
    public class UserService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IUserRepository _repo;
        private readonly IEmailService _emailService;
        private readonly ExternalApiServices _externalApiServices;

        // passar isto para o configurations file
        private readonly string emailBody = $"https://localhost:5000/api/Users/";

        public UserService(IUnitOfWork unitOfWork, IUserRepository repo, IEmailService emailService, ExternalApiServices externalApiServices)
        {
            this._unitOfWork = unitOfWork;
            this._repo = repo;
            this._emailService = emailService;
            this._externalApiServices = externalApiServices;
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
            int mechanographicNum = 0;

            if (dto.Role != Role.Patient)
            {
                mechanographicNum = await this._repo.GetLastMechanographicNumAsync() + 1;
            }

            var user = new User(dto.Role, dto.Email, mechanographicNum);

            await this._repo.AddAsync(user);

            await this._unitOfWork.CommitAsync();

            string messageBodyParameters = $"{user.Id.AsGuid()}?password=changeMeToYourNewPassword";
            try
            {
                await _emailService.SendEmail(dto.Email, emailBody + messageBodyParameters, "change your password");
            }
            catch (SmtpException ex) {
                Console.WriteLine(ex.Message);
            }
            return new UserDto
            {
                Id = user.Id.AsGuid(),
                Role = user.Role,
                Email = user.Email.ToString(),
                Password = user.Password
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

        internal async Task<LoginDTO> validateAuthorization(StringValues tokenHeader)
        {

            // Remove the 'Bearer ' prefix from the token
            var token = tokenHeader.ToString().Replace("Bearer ", string.Empty);

            LoginDTO loginDTO = new LoginDTO()
            {
                jwt = token
            };

            LoginDTO loginDTOResult =await _externalApiServices.validateToken(loginDTO);

            return loginDTOResult;
        }
    }
}