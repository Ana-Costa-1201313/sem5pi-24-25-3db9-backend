using Backoffice.Domain.Shared;

namespace Backoffice.Domain.Users
{
    public class UserService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IUserRepository _repo;

        public UserService(IUnitOfWork unitOfWork, IUserRepository repo)
        {
            this._unitOfWork = unitOfWork;
            this._repo = repo;
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            var list = await this._repo.GetAllAsync();

            List<UserDto> listDto = list.ConvertAll<UserDto>(u => new UserDto
            {
                Id = u.Id.AsGuid(),
                Username = u.Username,
                Role = u.Role,
                Email = u.Email,
                Password = u.Password
            });

            return listDto;
        }

        public async Task<UserDto> GetByIdAsync(UserId id)
        {
            var user = await this._repo.GetByIdAsync(id);

            if (user == null)
                return null;

            return new UserDto
            {
                Id = user.Id.AsGuid(),
                Username = user.Username,
                Role = user.Role,
                Email = user.Email,
                Password = user.Password
            };
        }

        public async Task<UserDto> AddAsync(CreateUserDto dto)
        {
            var user = new User(dto.Username, dto.Role, dto.Email);

            await this._repo.AddAsync(user);

            await this._unitOfWork.CommitAsync();

            return new UserDto
            {
                Id = user.Id.AsGuid(),
                Username = user.Username,
                Role = user.Role,
                Email = user.Email,
                Password = user.Password
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
                Username = user.Username,
                Role = user.Role,
                Email = user.Email,
                Password = user.Password,
                Active = user.Active
            };
        }
    }
}