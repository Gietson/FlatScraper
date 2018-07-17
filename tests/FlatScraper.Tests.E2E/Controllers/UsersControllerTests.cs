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
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.NotFound);
        }

        [Fact, TestPriority(1)]
        public async Task register_user_and_get_user_and_delete()
        {
            var newUser = new CreateUserDto
            {
                Email = "test123@email.com",
                Username = "test",
                Password = "secret",
                Role = "user"
            };
            var payload = GetPayload(newUser);
            var response = await Client.PostAsync("api/auth/register", payload);
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Created);
            response.Headers.Location.ToString().Should().BeEquivalentTo($"api/auth/register/{newUser.Email}");

            var user = await GetUserAsync(newUser.Email);
            user.Email.Should().BeEquivalentTo(newUser.Email);

            var responseDelete = await Client.DeleteAsync($"api/users/{user.Id}");
            responseDelete.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);

            var userAfterDelete = await GetUserAsync(newUser.Email);
            userAfterDelete.Should().Be(null);
        }
    }
}