using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using CleanArchitecture.API.ViewModels;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Infrastructure.Identity;
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
        public void CanSignInWithTestAccount()
        {
            var signInViewModel = new LoginViewModel()
            {
                Email = "test@loandepot.com",
                Password = "Testing@123",
                RememberMe = true
            };

            var jsonContent = new StringContent(JsonConvert.SerializeObject(signInViewModel), Encoding.UTF8,
                "application/json");

            var response = _client.PostAsync("/api/account/SignIn", jsonContent).Result;

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public void CanSignOut()
        {
            var response = _client.GetAsync("/api/account/SignOut").Result;
        
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        
        [Fact]
        public void CanAddClaims()
        {
            var response = _client.GetAsync("/api/account/ListClaims").Result;
        
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public void CanGenerateJwtToken()
        {
            //var appUser = new ApplicationUser()
            //{

            //}
                

            string invalidId = "100";
            var response = _client.GetAsync($"/api/guestbook/{invalidId}").Result;

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var stringResponse = response.Content.ReadAsStringAsync().Result;

            Assert.Equal(invalidId.ToString(), stringResponse);
        }
    }
}