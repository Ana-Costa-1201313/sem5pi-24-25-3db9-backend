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
using Microsoft.AspNetCore.Mvc;
using Google.Apis.Auth;


namespace Auth.Domain.Users
{
    public class LoginService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _iconfig;
        private readonly ExternalApiService _externalApiService;


        public LoginService(IConfiguration iConfig, IUnitOfWork unitOfWork, IUserRepository userRepository, ExternalApiService externalApiService)
        {
            this._unitOfWork = unitOfWork;
            this._userRepository = userRepository;
            this._iconfig = iConfig;
            _externalApiService = externalApiService;
        }

        public async Task<bool> validToken(String loginDTOJWT) {
            if (string.IsNullOrEmpty(loginDTOJWT))
            {
                return false;
            }

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _iconfig["Jwt:Issuer"],

                ValidateAudience = true,
                ValidAudience = _iconfig["Jwt:Audience"],

                ValidateLifetime = true,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_iconfig["Jwt:Key"])),
            };

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                ClaimsPrincipal principal = tokenHandler.ValidateToken(loginDTOJWT, tokenValidationParameters, out SecurityToken validatedToken);

                if (validatedToken is JwtSecurityToken jwtToken && jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<ActionResult<LoginDTO>> login(String email, String password)
        {
            // make request to Backoffice api
            LoginDTO loginDTO = null;
            string token = null;
            try 
            { 
            loginDTO = await _externalApiService.UserExists(email, password);
            }
            catch (Exception ex) {
                throw new NullReferenceException(ex.Message );
            }
            //good request -> createToken() -> return Token(username, password, role,....)
            if (loginDTO != null) token = CreateToken(loginDTO);
            loginDTO.jwt = token;

            return loginDTO;
        }

        public async Task<LoginDTO> loginGoogle(String credential)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { this._iconfig.GetValue<string>("GoogleCIientId") }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(credential, settings);

            String email = payload.Email;

            LoginDTO loginDTO = await _externalApiService.UserExists(email);

            if (loginDTO != null) return null;
            //return UtilizadorMapper.toDTO(user, GerarJWT(user.userName.id, user.PermissaoString));
            throw new Exception("Erro: Não existe Utilizador");
        }

        public String CreateToken(LoginDTO loginDTO)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            //var key = Encoding.ASCII.GetBytes(_iconfig["Jwt:Key"]);
            var jwtSettings = _iconfig.GetSection("Jwt");
            var key = _iconfig["Jwt:Key"];
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, loginDTO.username),
                    new Claim(ClaimTypes.Role, loginDTO.role),
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

            return tokenHandler.WriteToken(token);
        }

        public String CreateToken1(LoginDTO loginDTO)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            //var key = Encoding.ASCII.GetBytes(_iconfig["Jwt:Key"]);
            var jwtSettings = _iconfig.GetSection("Jwt");
            var key = _iconfig["Jwt:Key"];
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, loginDTO.username),
                    new Claim(ClaimTypes.Role, loginDTO.role),
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

            return tokenHandler.WriteToken(token);
        }

        

        public LoginDTO ExtractLoginDTOFromToken(LoginDTO loginDTO)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(loginDTO.jwt);

            loginDTO.username = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;
            loginDTO.role = jwtToken.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
            loginDTO.jwt = loginDTO.jwt;

            return loginDTO;
        }
    }
}