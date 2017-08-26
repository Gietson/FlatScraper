using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FlatScraper.Infrastructure.DTO;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace FlatScraper.Tests.E2E.Controllers
{
    public class ScanPageControllerTests : ControllerTestsBase
    {
        private async Task<IEnumerable<ScanPageDto>> GetAllAsync()
        {
            var response = await Client.GetAsync("api/scanpage");
            var responseString = await response.Content.ReadAsStringAsync();

            var pages = JsonConvert.DeserializeObject<IEnumerable<ScanPageDto>>(responseString);
            return pages;
        }

        [Fact]
        public async Task add_new_scanpage()
        {
            var page = new ScanPageDto()
            {
                UrlAddress = "https://www.gumtree.pl/s-mieszkania-i-domy-sprzedam-i-kupie/warszawa/v1c9073l3200008p2",
                Page = "Gumtree"
            };
            var payload = GetPayload(page);
            var response = await Client.PostAsync("api/scanpage", payload);
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);

            var pages = await GetAllAsync();

            Assert.NotEmpty(pages);
        }

        [Fact]
        public async Task delete_new_scanpage()
        {
            var pages = await GetAllAsync();
            Assert.NotEmpty(pages);

            Guid id = pages.FirstOrDefault().Id;
            //var payload = GetPayload(id);
            var response = await Client.DeleteAsync($"api/scanpage/{id}");
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);
        }
    }
}