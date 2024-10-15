using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Backoffice.Domain.Users;

namespace Backoffice.Services
{
    public class ExternalApiServices
    {

        protected string validateUrl = "validate";

        private readonly IConfiguration _iconfig;
        private HttpClient _httpClient;


        public ExternalApiServices(IConfiguration iConfig, HttpClient httpClient)
        {
            this._iconfig = iConfig;
            this._httpClient = httpClient;
        }

        public async Task<LoginDTO> validateToken(LoginDTO loginDTO)
        {
            try
            {
                // Create a GET request message
                var request = new HttpRequestMessage(HttpMethod.Get, _iconfig["Auth:urlLocal"]+validateUrl);

                // Add the token to the headers
                request.Headers.Add("Authorization", $"Bearer {loginDTO.jwt}");

                //  Send
                HttpResponseMessage response = await _httpClient.SendAsync(request);

                response.EnsureSuccessStatusCode(); // Throw an exception if the response status code is not successful

                // Deserialize the JSON response content to a LoginDTO object
                string responseBody = await response.Content.ReadAsStringAsync();
                LoginDTO resultLoginDTO = JsonSerializer.Deserialize<LoginDTO>(responseBody);

                return resultLoginDTO;
            }
            catch (Exception ex)
            {
                throw new NullReferenceException("Token not valid");
            }
        }

    }
}
