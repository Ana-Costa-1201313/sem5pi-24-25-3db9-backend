using System.Threading.Tasks;
using System.Collections.Generic;
using Auth.Domain.Shared;
using Auth.Infrastructure.Users;
using Auth.Domain.Users.DTO;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;


namespace Auth.Domain.Users {
    public class LoginService{

        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _iconfig;


        public LoginService(IConfiguration iConfig, IUnitOfWork unitOfWork, IUserRepository userRepository) {
            this._unitOfWork = unitOfWork;
            this._userRepository = userRepository;
            this._iconfig = iConfig;
        }

        //public async Task<UserDTO> public Login(LoginDTO loginDto){
        //    if (loginDto.userName != null && loginDto.password != null)
        //        return Ok(await _service.login(loginDto.userName, loginDto.password));
        //    return BadRequest(new {Message = "Body no formato errado." });
        //}


        private String GenerateJWT(string userName, string permissoes)
        {
            //TODO Gerar JWT
            return userName;
        }

        public SecurityToken CreateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            //var key = Encoding.ASCII.GetBytes(_iconfig["Jwt:Key"]);
            var jwtSettings = _iconfig.GetSection("Jwt");
            var key = _iconfig["Jwt:Key"];
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.username.username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Ensures a unique token identifier
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(), ClaimValueTypes.Integer64)
        
                    //new Claim(ClaimTypes.Role, user.Role.ToString()),
                    // Add more claims as needed
                }),
                Expires = DateTime.UtcNow.AddHours(72),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)), SecurityAlgorithms.HmacSha256),
                Issuer = _iconfig["Jwt:Issuer"],
                Audience = _iconfig["Jwt:Audience"]
            };


            var token = tokenHandler.CreateToken(tokenDescriptor);

            //return tokenHandler.WriteToken(token);
            return token;
        }
    }
}