using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FlatScraper.API;
using FlatScraper.Infrastructure.DTO;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Xunit;

namespace FlatScraper.Tests.E2E.Controllers
{
    public class AdControllerTests : ControllerTestsBase
    {

        /*[Fact]
        public async Task get_all_users()
        {
            var response = await Client.GetAsync("api/users");
            var responseString = await response.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<IEnumerable<UserDto>>(responseString);

            Assert.NotEmpty(users);
        }

        [Fact]
        public async Task given_invalid_email_user_should_not_exist()
        {
            string email = "user1000@email.com";
            var response = await Client.GetAsync($"api/users/{email}");
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.NotFound);
        }*/

        [Fact]
        public async Task add_new_ads()
        {
            var ad = new InsertAdDto() { Url = "https://www.gumtree.pl/s-mieszkania-i-domy-sprzedam-i-kupie/warszawa/v1c9073l3200008p1" };
            var payload = GetPayload(ad);
            var response = await Client.PostAsync("api/ad", payload);
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);

            var ads = await GetAllAsync();
            Assert.NotEmpty(ads);
            /*
            var user = await GetUserAsync(newUser.Email);
            user.Email.ShouldBeEquivalentTo(newUser.Email);

            var responseDelete = await Client.DeleteAsync($"api/users/{user.Id}");
            responseDelete.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);

            var userAfterDelete = await GetUserAsync(newUser.Email);
            userAfterDelete.ShouldBeEquivalentTo(null);*/
        }

        private async Task<IEnumerable<AdDto>> GetAllAsync()
        {
            var response = await Client.GetAsync("api/ad");
            var responseString = await response.Content.ReadAsStringAsync();

            var ads = JsonConvert.DeserializeObject<IEnumerable<AdDto>>(responseString);
            return ads;
        }
    }

}