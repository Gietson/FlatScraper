using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FlatScraper.Infrastructure.DTO;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace FlatScraper.Tests.E2E.Controllers
{
    public class ScraperControllerTests : ControllerTestsBase
    {
        private async Task<ScanPageDto> Get(Guid id)
        {
            var response = await Client.GetAsync($"api/scanpage/{id}");
            var responseString = await response.Content.ReadAsStringAsync();

            var page = JsonConvert.DeserializeObject<ScanPageDto>(responseString);
            return page;
        }

        private async Task<IEnumerable<ScanPageDto>> GetAllAsync()
        {
            var response = await Client.GetAsync("api/scanpage");
            var responseString = await response.Content.ReadAsStringAsync();

            var pages = JsonConvert.DeserializeObject<IEnumerable<ScanPageDto>>(responseString);
            return pages;
        }

        [Fact]
        public async Task scrap_all_pages()
        {
            var response = await Client.GetAsync("api/scrap");
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);

            var pages = await GetAllAsync();

            Assert.NotEmpty(pages);
        }
    }
}