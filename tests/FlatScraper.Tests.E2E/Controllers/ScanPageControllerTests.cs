using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FlatScraper.Infrastructure.DTO;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace FlatScraper.Tests.E2E.Controllers
{
    [TestCaseOrderer("FullNameOfOrderStrategyHere", "OrderStrategyAssemblyName")]
    public class ScanPageControllerTests : ControllerTestsBase
    {
        private string urlAddress;
        private string newUrl;

        public ScanPageControllerTests()
        {
            urlAddress = "https://www.gumtree.pl/s-mieszkania-i-domy-sprzedam-i-kupie/warszawa/v1c9073l3200008p2";
            newUrl = "www.test.com";
        }

        private async Task<ScanPageDto> Get(Guid id)
        {
            var response = await Client.GetAsync($"api/scanpage/{id}");
            var responseString = await response.Content.ReadAsStringAsync();

            var page = JsonConvert.DeserializeObject<ScanPageDto>(responseString);
            return page;
        }
        private async Task<ScanPageDto> Get(string urlAddress)
        {
            var response = await Client.GetAsync($"api/scanpage/{urlAddress}");
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

        [Fact, TestPriority(1)]
        public async Task add_new_scanpage()
        {
            var page = new ScanPageDto()
            {
                UrlAddress = urlAddress,
                Page = "Gumtree"
            };
            var payload = GetPayload(page);
            var response = await Client.PostAsync("api/scanpage", payload);
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);

            var pages = await GetAllAsync();

            Assert.NotEmpty(pages);
        }

        [Fact, TestPriority(2)]
        public async Task change_scanpage_and_get_by_id()
        {
            ScanPageDto page = await Get(urlAddress);
            page.Active = false;
            page.UrlAddress = newUrl;

            var payload = GetPayload(page);
            var response = await Client.PutAsync($"api/scanpage", payload);
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);

            ScanPageDto newPage = await Get(newUrl);
            newPage.UrlAddress.ShouldBeEquivalentTo(newUrl);
        }

        [Fact, TestPriority(3)]
        public async Task delete_new_scanpage()
        {
            HttpResponseMessage response;
            var page = new ScanPageDto()
            {
                UrlAddress = "https://www.olx.pl/nieruchomosci/mieszkania/sprzedaz/warszawa/?page=2",
                Page = "Olx"
            };
            var payload = GetPayload(page);
            response = await Client.PostAsync("api/scanpage", payload);
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);

            var pages = await GetAllAsync();
            Assert.NotEmpty(pages);

            Guid id = pages.FirstOrDefault().Id;
            response = await Client.DeleteAsync($"api/scanpage/{id}");
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);
        }
    }
}