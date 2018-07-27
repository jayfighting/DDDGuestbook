using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace CleanArchitecture.Tests.Integration.API
{
    public class BasicTests
        : IClassFixture<WebApplicationFactory<CleanArchitecture.API.Startup>>
    {
        private readonly WebApplicationFactory<CleanArchitecture.API.Startup> _factory;

        public BasicTests(WebApplicationFactory<CleanArchitecture.API.Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/api/Todo")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299

            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Get_SecurePageRequiresAnAuthenticatedUser()
        {
            // Arrange
            var client = _factory.CreateClient(
                new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });

            // Act
            var response = await client.GetAsync("/SecurePage");

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.StartsWith("http://localhost/Identity/Account/Login", 
                response.Headers.Location.OriginalString);
        }
    }
}