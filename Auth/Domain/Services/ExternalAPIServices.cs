using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Auth.Domain.Users.DTO;

public class ExternalApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ExternalApiService> _logger;
    private readonly IConfiguration _iconfig;

    private readonly string _url_loginVerification = "Backoffice/Login/userExists";

    public ExternalApiService(HttpClient httpClient, ILogger<ExternalApiService> logger, IConfiguration iconfig)
    {
        _httpClient = httpClient;
        _logger = logger;
        _iconfig = iconfig;
    }

    public async Task<LoginDTO> UserExists(string email, string password)
    {
        LoginDTO loginDTO = new LoginDTO 
        {
            username = email, password = password
        };
        try
        {
            // Send a GET request to the specified API

            var request = new HttpRequestMessage(HttpMethod.Get, _iconfig["Backoffice:uri"] + _url_loginVerification);

            // Add the email,password to the headers
            string credentials = $"{email}:{password}";

            byte[] credentialsBytes = Encoding.UTF8.GetBytes(credentials);

            request.Headers.Add("Authorization", $"Basic {Convert.ToBase64String(credentialsBytes)}");

            //  Send
            HttpResponseMessage response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode(); // Throw an exception if the response status code is not successful

            // Read the response content as a string
            string responseBody = await response.Content.ReadAsStringAsync();
            LoginDTO resultLoginDTO = JsonSerializer.Deserialize<LoginDTO>(responseBody);

       
            return resultLoginDTO;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error calling external API: {ex.Message}");
            throw;
        }
    }

    public async Task<string> PostDataToExternalApiAsync(string apiUrl, object data)
    {
        try
        {
            // Convert the data object to JSON
            var jsonContent = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(data),
                System.Text.Encoding.UTF8,
                "application/json"
            );

            // Send a POST request to the specified API with the JSON data
            HttpResponseMessage response = await _httpClient.PostAsync(apiUrl, jsonContent);

            response.EnsureSuccessStatusCode(); // Throw an exception if the response status code is not successful

            // Read the response content as a string
            string responseData = await response.Content.ReadAsStringAsync();

            return responseData;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error posting to external API: {ex.Message}");
            throw;
        }
    }
}
