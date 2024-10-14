using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Auth.Domain.Users.DTO;

public class ExternalApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ExternalApiService> _logger;

    public ExternalApiService(HttpClient httpClient, ILogger<ExternalApiService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<string> GetDataFromExternalApiAsync(string apiUrl)
    {
        try
        {
            // Send a GET request to the specified API
            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

            response.EnsureSuccessStatusCode(); // Throw an exception if the response status code is not successful

            // Read the response content as a string
            string responseData = await response.Content.ReadAsStringAsync();

            return responseData;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error calling external API: {ex.Message}");
            throw;
        }
    }

    //public async Task<bool> UserExistsAndAccountIsActive(UserDTO userDTO)
    //{
    //    try 
    //    { 

    //    } 
    //    catch (Exception e) 
    //    { 

    //    }
    //}

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
