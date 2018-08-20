using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using CleanArchitecture.API.ViewModels;
using CleanArchitecture.Core.Entities;
using Newtonsoft.Json;
using Xunit;

namespace CleanArchitecture.Tests.Integration.API
{

    public class ApiAccountControllerShould : BaseWebTest
    {
        [Fact]
        public void RegisterIfNotPresent()
        {
            var registerViewModel = new RegisterViewModel()
            {
                Email = "Hzhao@loandepot.com",
                Password = "Testing@123",
                ConfirmPassword = "Testing@123"
            };

            var jsonContent = new StringContent(JsonConvert.SerializeObject(registerViewModel), Encoding.UTF8,
                "application/json");

            var response = _client.PostAsync("/api/account/register", jsonContent).Result;

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public void CanGet()
        {

            var response = _client.GetAsync($"/api/account/Protected").Result;
            var stringResponse = response.Content.ReadAsStringAsync().Result;

            //Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(stringResponse);
        }

        [Fact]
        public void Return404GivenInvalidId()
        {
            string invalidId = "100";
            var response = _client.GetAsync($"/api/guestbook/{invalidId}").Result;

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var stringResponse = response.Content.ReadAsStringAsync().Result;

            Assert.Equal(invalidId.ToString(), stringResponse);
        }
    }
}