using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Backoffice.Domain.Users;

namespace Backoffice.Services
{
    public class ExternalApiServices
    {

        protected string validateUrl = "validate";

        private readonly IConfiguration _iconfig;


        public ExternalApiServices(IConfiguration iConfig)
        {
            this._iconfig = iConfig;
        }

        public async Task<bool> validateToken(LoginDTO loginDTO)
        {
            try
            {
                // Create a GET request message
                var request = new HttpRequestMessage(HttpMethod.Get, _iconfig["Auth:urlLocal"]+ validateUrl);

                // Add the token to the headers
                request.Headers.Add("Authorization", $"Bearer {loginDTO.jwt}");

                //  Send
                // HttpResponseMessage response = await httpClient.SendAsync(request);

                // response.EnsureSuccessStatusCode(); // Throw an exception if the response status code is not successful

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
