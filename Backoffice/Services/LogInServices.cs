using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Backoffice.Domain.Users;

namespace Backoffice.Services
{
    public class LogInServices
    {
        protected string validateUrl = "validate";

        private readonly IConfiguration _iconfig;
        private readonly IUserRepository _userRepository;


        public LogInServices(IConfiguration iConfig, IUserRepository userRepository)
        {
            this._iconfig = iConfig;
            this._userRepository = userRepository;
        }

        public async Task<bool> CheckUserCredentials1(string username, string password)
        {
            User user = await _userRepository.getUserByEmail(username);
            if (user == null) return false;
            if (password.Equals(user.Password.Passwd)) return false;
            return true;
        }
    }
}