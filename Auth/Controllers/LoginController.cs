using Microsoft.AspNetCore.Mvc;
using Auth.Domain.Users;
using Auth.Domain.Users.DTO;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.AspNetCore.Authorization;

using Newtonsoft.Json; // For JsonConvert
using System.Text; // For Encoding

namespace Auth.Controllers
{

    [Route("Auth/[Controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private readonly LoginService _service;

        public LoginController(LoginService service) // IAppService
        {
            _service = service;
        }


        [HttpPost("loginDto")]
        public async Task<ActionResult<LoginDTO>> login([FromBody] LoginDTO loginDto)
        {

            try
            {
                if (loginDto.jwt != null)
                {
                    if (await _service.validToken(loginDto.jwt))
                        return Ok(loginDto);
                }
                else if (loginDto.googleCredentials != null)
                    return Ok(await _service.loginGoogle(loginDto.googleCredentials));
                else if (loginDto.username != null && loginDto.password != null)
                {
                    var result = await _service.login(loginDto.username, loginDto.password);
                    if (result == null) return BadRequest(new { Message = "LoginController - loginDto é null" });
                    return Ok(result);
                }
                //}else if (loginDto.username != null && loginDto.password != null)
                //{
                //    var result = await _service.login(loginDto.username, loginDto.password);
                //    return Ok(result);
                //}
                //return BadRequest(new { Message = "mau boas." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return BadRequest(new { Message = "An error occurred during login." });
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<String> loginGoogle()
        {
            var user = this.User;

            return "value";
        }

        //[HttpGet("{id}")]
        //public async Task<ActionResult<LoginDTO>> teste(string id) 
        //{
        //    var l = new LoginDTO
        //    {
        //        username = "ricardo",
        //        role = "student"
        //    };

        //    var login = new LoginDTO
        //    {
        //        //jwt = _service.CreateToken(new Domain.Users.User("ricardo1", "student"))
        //        jwt = _service.CreateToken(l)
        //    };

        //    return Ok(login);
        //}

        [HttpPost("validate")]
        public async Task<ActionResult<LoginDTO>> validateToken()
        //public async Task<ActionResult<LoginDTO>> validateToken(LoginDTO loginDTO)
        {

            try
            {
                // Read the token from the request header
                if (!Request.Headers.TryGetValue("Authorization", out var tokenHeader))
                {
                    return BadRequest("Authorization header is missing");
                }

                // Remove the 'Bearer ' prefix from the token
                var token = tokenHeader.ToString().Replace("Bearer ", string.Empty);

                // Use the validateToken method to check the token's validity
                bool valid = await _service.validToken(token);
                //bool valid = await _service.validToken(loginDTO.jwt);

                LoginDTO loginDTO = new LoginDTO(){jwt = token };
                if (valid)
                {
                    loginDTO = _service.ExtractLoginDTOFromToken(loginDTO);
                    return Ok(loginDTO);
                }
                else
                {
                    return Unauthorized("Invalid token");
                }
            }
            catch (Exception ex)
            {
                // Return a generic error message for any exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }

    [Route("/")]
    //[Route("{*url}"]
    [ApiController]
    public class GoogleLoginController : ControllerBase
    {
        //[HttpGet("{variable}")]
        //public async Task<String> GoogleLogin()
        //{
        //    return variable;
        //}

        //[HttpGet("{variable}")]
        //public async Task<IActionResult> GoogleLogin(string variable)
        //{
        //    // Simulating an asynchronous operation (for example, fetching user info)
        //    await Task.Delay(100); // Simulate some async work, like a database call.

        //    return Ok(variable); // Return the variable as a response.
        //}   
        private readonly HttpClient _httpClient;

        public GoogleLoginController(HttpClient httpClient)
        {
            _httpClient = new HttpClient(); ;
        }

        [HttpPost]
        public async Task<IActionResult> ExchangeCredentials([FromBody] TokenRequest tokenRequest)
        {
            //    if (tokenRequest == null)
            //    {
            //        return BadRequest("Token request cannot be null.");
            //    }

            //    string url = "https://sts.googleapis.com/v1/token";

            //    // Create the request body as a JSON object
            //    var requestBody = new
            //    {
            //        audience = tokenRequest.Audience,  // e.g., https://your-api-endpoint
            //        grant_type = "urn:ietf:params:oauth:grant-type:jwt-bearer",
            //        assertion = tokenRequest.Assertion   // JWT or other credentials
            //    };

            //    var json = JsonConvert.SerializeObject(requestBody);
            //    var content = new StringContent(json, Encoding.UTF8, "application/json");

            //    try
            //    {
            //        // Make the POST request
            //        HttpResponseMessage response = await _httpClient.PostAsync(url, content);

            //        // Check if the response is successful
            //        if (response.IsSuccessStatusCode)
            //        {
            //            var responseData = await response.Content.ReadAsStringAsync();
            //            return Ok(responseData); // Return the access token
            //        }
            //        else
            //        {
            //            // Return the error response from Google API
            //            var errorResponse = await response.Content.ReadAsStringAsync();
            //            return StatusCode((int)response.StatusCode, errorResponse);
            //        }
            //    }
            //    catch (HttpRequestException httpEx)
            //    {
            //        return StatusCode(500, $"Request error: {httpEx.Message}");
            //    }
            //    catch (Exception ex)
            //    {
            //        return StatusCode(500, $"Internal server error: {ex.Message}");
            //    }

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://sts.googleapis.com//v1/token?$.xgafv=<string>&access_token=<string>&alt=<string>&callback=<string>&fields=<string>&key=<string>&oauth_token=<string>&prettyPrint=<boolean>&quotaUser=<string>&upload_protocol=<string>&uploadType=<string>");
            var content = new StringContent("{\n    \"grantType\": \"<string>\",\n    \"options\": \"<string>\",\n    \"requestedTokenType\": \"<string>\",\n    \"subjectToken\": \"<string>\",\n    \"subjectTokenType\": \"<string>\"\n}", null, "application/json");
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            Console.WriteLine(await response.Content.ReadAsStringAsync());

            return Ok(response);
        }

    }

    public class TokenRequest
    {
        public string Audience { get; set; }
        public string Assertion { get; set; }
    }
}




        