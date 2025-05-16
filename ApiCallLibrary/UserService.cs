using ApiCallLibrary.DTO;
using ApiCallLibrary.IUserService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Text.Json;

namespace ApiCallLibrary
{
    public class UserService : IUserInterface
    {

        private readonly HttpClient _httpClient;
        private readonly ApiSettings _apiSettings;
      
   


        public UserService(HttpClient httpClient, IOptions<ApiSettings> options)
        {
            _httpClient = httpClient;
            _apiSettings = options.Value;
        }

        public async Task<IEnumerable<User>?> GetAllUsersAsync(int pNo)
        {
            
            var users = new List<User>();
            int currentPage = 1;
            int totalPages=0;
            try
            {
                do
                {
                    var url = $"{_apiSettings.BaseUrl}/users?page={currentPage}";
                    var response = await _httpClient.GetAsync(url);
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new HttpRequestException($"Error getting records for page {currentPage}, StatusCode: {response.StatusCode}");
                    }

                    var content = await response.Content.ReadAsStringAsync();
                    var pageData = JsonSerializer.Deserialize<BulkResponse>(content);

                    if (pageData?.Data != null)
                    {
                        users.AddRange(pageData.Data);
                        totalPages++;
                    }
                    else
                    {
                        break;
                    }

                    currentPage++;
                } while (currentPage <= pNo);

                return users;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Network error while fetching user.", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception("Error while deserialization", ex);
            }
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            
            var url = $"{_apiSettings.BaseUrl}/users/{userId}";
            Console.WriteLine(url);
            try
            {
                var response = await _httpClient.GetAsync(url);

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return null;


                var contentStream = await response.Content.ReadAsStreamAsync();
                var userResponse = await JsonSerializer.DeserializeAsync<User>(
                    contentStream);

                return userResponse;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Network error while fetching user.", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception("Error while deserialization", ex);
            }
        }
    }
    
}
