using Moq.Protected;
using Moq;
using System.Net;
using ApiCallLibrary.DTO;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace ApiCallLibrary.Test
{
    public class UserServiceTest
    {
        private static HttpClient CreateMockHttpClient(HttpResponseMessage response)
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            return new HttpClient(handlerMock.Object);
        }

        private static IOptions<ApiSettings> GetFakeOptions() =>
            Options.Create(new ApiSettings { BaseUrl = "https://reqres.in/api/Users/1" });



        [Fact]
        public async Task GetUserByIdAsync_ReturnsUser_WhenResponseIsSuccessful()
        {
            var expectedUser = new User
            {
                Id = 1,
                First_Name = "Vishwesh",
                Last_Name = "Manerkar",
                Email = "vishwesh.manerkar@gmail.com"
            };

            var json = JsonSerializer.Serialize(new { data = expectedUser });
            var mockHttp = CreateMockHttpClient(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(json)
            });

            var service = new UserService(mockHttp, GetFakeOptions());

            // Act
            var result = await service.GetUserByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUser.Id, result.Id);
            Assert.Equal(expectedUser.First_Name, result.First_Name);
        }


        [Fact]
        public async Task GetUserByIdAsync_ReturnsNull_WhenNotFound()
        {
            var mockHttp = CreateMockHttpClient(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound
            });

            var service = new UserService(mockHttp, GetFakeOptions());

            var result = await service.GetUserByIdAsync(99);

            Assert.Null(result);
        }
    }
    
}