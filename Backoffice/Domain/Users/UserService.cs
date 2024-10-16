using Backoffice.Domain.Shared;
using Backoffice.Domain.Staffs;

namespace Backoffice.Domain.Users
{
    public class UserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _repo;
        private readonly IStaffRepository _staffRepo;

        public UserService(IUnitOfWork unitOfWork, IUserRepository repo, IStaffRepository staffRepo)
        {
            this._unitOfWork = unitOfWork;
            this._repo = repo;
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

            //depois do user estar criado envia o link para o confirmation email do dto para o user definir a password

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
    }
}