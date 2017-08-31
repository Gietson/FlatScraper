using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FlatScraper.Infrastructure.DTO;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace FlatScraper.Tests.E2E.Controllers
{
    public class UsersControllerTests : ControllerTestsBase
    {
        private async Task<UserDto> GetUserAsync(string email)
        {
            var response = await Client.GetAsync($"api/users/{email}");
            var responseString = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<UserDto>(responseString);
        }

        [Fact, TestPriority(1)]
        public async Task register_user_and_get_user_and_delete()
        {
            var newUser = new CreateUserDto
            {
                Email = "test@email.com",
                Username = "test",
                Password = "secret",
                Role = "user"
            };
            var payload = GetPayload(newUser);
            var response = await Client.PostAsync("api/users", payload);
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.Created);
            response.Headers.Location.ToString().ShouldBeEquivalentTo($"api/users/{newUser.Email}");

            var user = await GetUserAsync(newUser.Email);
            user.Email.ShouldBeEquivalentTo(newUser.Email);

            var responseDelete = await Client.DeleteAsync($"api/users/{user.Id}");
            responseDelete.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);

            var userAfterDelete = await GetUserAsync(newUser.Email);
            userAfterDelete.ShouldBeEquivalentTo(null);
        }
        /*
        [Fact, TestPriority(2)]
        public async Task get_all_users()
        {
            var response = await Client.GetAsync("api/users");
            var responseString = await response.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<IEnumerable<UserDto>>(responseString);

            Assert.NotEmpty(users);
        }
        */
        [Fact, TestPriority(2)]
        public async Task given_invalid_email_user_should_not_exist()
        {
            string email = "user1000@email.com";
            var response = await Client.GetAsync($"api/users/{email}");
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.NotFound);
        }

    }
}