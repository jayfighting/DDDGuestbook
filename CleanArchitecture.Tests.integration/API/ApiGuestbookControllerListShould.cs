using System.Linq;
using System.Net;
using CleanArchitecture.Core.Entities;
using Newtonsoft.Json;
using Xunit;

namespace CleanArchitecture.Tests.Integration.API
{

    public class ApiGuestbookControllerListShould : BaseWebTest
    {
        [Fact]
        public void ReturnGuestbookWithOneItem()
        {
            var response = _client.GetAsync("/api/guestbook/1").Result;
            response.EnsureSuccessStatusCode();
            var stringResponse = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<Guestbook>(stringResponse);

            Assert.Equal(1, result.Id);
            Assert.Equal(1, result.Entries.Count());
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