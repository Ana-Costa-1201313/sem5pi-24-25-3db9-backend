using System.Threading.Tasks;
using System.Collections.Generic;
using Auth.Domain.Shared;
using Auth.Infrastructure.Users;

namespace Auth.Domain {

    public class UserService {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _repo;

        public UserService(IUnitOfWork unitOfWork, IUserRepository repo)
        {
            this._unitOfWork = unitOfWork;
            this._repo = repo;
        }
    }

    // public async Task<UserDTO> login(String username, String password)
    //{
    // if (username == "admin" && password == "admin")
    // {
    //     if (!_repo.UserExists(username))
    //     {
    // 
    //     }
    // 
    //     User user = await _repo.GetByUserName(username);
    //     if (user != null)
    //         return UtilizadorMapper.toDTO(user, GerarJWT(userName, user.PermissaoString));
    // }
    // 
    // throw new Exception("Erro: Email e/ou Password inv�lidos!");
    //}


}