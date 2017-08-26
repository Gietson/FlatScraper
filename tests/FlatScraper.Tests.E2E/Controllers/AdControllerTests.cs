using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FlatScraper.Infrastructure.DTO;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace FlatScraper.Tests.E2E.Controllers
{
    public class AdControllerTests : ControllerTestsBase
    {
        private async Task<IEnumerable<AdDto>> GetAllAsync()
        {
            var response = await Client.GetAsync("api/ad");
            var responseString = await response.Content.ReadAsStringAsync();

            var ads = JsonConvert.DeserializeObject<IEnumerable<AdDto>>(responseString);
            return ads;
        }

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
        public async Task add_new_ads_gumtree()
        {
            var ad = new InsertAdDto()
            {
                Url = "https://www.gumtree.pl/s-mieszkania-i-domy-sprzedam-i-kupie/warszawa/v1c9073l3200008p1"
            };
            var payload = GetPayload(ad);
            var response = await Client.PostAsync("api/ad", payload);
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);

            var ads = await GetAllAsync();
            Assert.NotEmpty(ads);
        }

        //[Fact]
        //public async Task add_new_ads_olx()
        //{
        //    var ad = new InsertAdDto()
        //    {
        //        Url = "https://www.olx.pl/nieruchomosci/mieszkania/sprzedaz/warszawa/"
        //    };
        //    var payload = GetPayload(ad);
        //    var response = await Client.PostAsync("api/ad", payload);
        //    response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);

        //    var ads = await GetAllAsync();
        //    Assert.NotEmpty(ads);

        //}
    }
}