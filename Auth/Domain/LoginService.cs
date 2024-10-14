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


namespace Auth.Domain.Users
{
    public class LoginService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _iconfig;


        public LoginService(IConfiguration iConfig, IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            this._unitOfWork = unitOfWork;
            this._userRepository = userRepository;
            this._iconfig = iConfig;
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

                ClockSkew = TimeSpan.Zero // Remove the default clock skew of 5 minutes
            };

            try
            {
                // Create a token handler
                var tokenHandler = new JwtSecurityTokenHandler();

            // Validate the token
            ClaimsPrincipal principal = tokenHandler.ValidateToken(loginDTOJWT, tokenValidationParameters, out SecurityToken validatedToken);

                // Check if the validated token is a JwtSecurityToken
                if (validatedToken is JwtSecurityToken jwtToken && jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    // Token is valid
                    return true;
                }
                else
                {
                    // Invalid token
                    return false;
                }
            }
            catch (Exception)
            {
                // Token validation failed
                return false;
            }
        }

        public async Task<ActionResult<UserDTO>> login(String username, String password) {
            return null;
        }

        public String CreateToken(User user)
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
            return tokenHandler.WriteToken(token);
        }
    }
}