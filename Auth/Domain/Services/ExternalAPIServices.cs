using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Auth.Domain.Users.DTO;
using System.Text;
using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class ExternalApiService
{
    private readonly HttpClient _httpClient;
    private readonly HttpClient httpClient;
    private readonly ILogger<ExternalApiService> _logger;
    private readonly IConfiguration _iconfig;

    private readonly string _url_loginVerificationEmailPassword = "Backoffice/Login/userExists";
    //private readonly string _url_loginVerificationEmailPassword = "Backoffice/Login/userExists";

    public ExternalApiService(HttpClient httpClient, ILogger<ExternalApiService> logger, IConfiguration iconfig)
    {
        _httpClient = httpClient;
        _logger = logger;
        _iconfig = iconfig;
        //httpClient = new HttpClient();
    }

    public async Task<LoginDTO> UserExists(string email, string password)
    {
        // Set the base URL for the HTTP client
        HttpClient httpClient = new HttpClient();

        // Create the Basic Authentication token
        var authToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{email}:{password}"));

        var a = string.Concat(_iconfig["BackOffice:Uri"], _url_loginVerificationEmailPassword);

        using (var request = new HttpRequestMessage(HttpMethod.Post, a))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authToken);
            var response = await httpClient.SendAsync(request);
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to authenticate user:+ {ex.Message} + {a} + {authToken}+ {email}+,+{password}");
            }
            var responseContent = await response.Content.ReadAsStringAsync();
            var loginDto = JsonSerializer.Deserialize<LoginDTO>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false
            });
            return loginDto;
        }
        return null;
    }

    public async Task<LoginDTO> UserExists(string email)
    {
        // Set the base URL for the HTTP client
        HttpClient httpClient = new HttpClient();

        // Create the Basic Authentication token
        var authToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{email}"));

        var a = string.Concat(_iconfig["BackOffice:Uri"], _url_loginVerificationEmailPassword);

        using (var request = new HttpRequestMessage(HttpMethod.Post, a))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authToken);
            var response = await httpClient.SendAsync(request);
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to authenticate user:+ {ex.Message} + {a} + {authToken}+ {email}");
            }
            var responseContent = await response.Content.ReadAsStringAsync();
            var loginDto = JsonSerializer.Deserialize<LoginDTO>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false
            });
            return loginDto;
        }
        return null;
    }

}
