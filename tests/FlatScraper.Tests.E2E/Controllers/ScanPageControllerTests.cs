﻿using FlatScraper.Infrastructure.DTO;
using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace FlatScraper.Tests.E2E.Controllers
{
    [TestCaseOrderer("FullNameOfOrderStrategyHere", "OrderStrategyAssemblyName")]
    public class ScanPageControllerTests : ControllerTestsBase
    {
        private string urlAddress;
        private string newUrl;
        private string uri;
        public ScanPageControllerTests()
        {
            
            urlAddress = "https://www.gumtree.pl/s-mieszkania-i-domy-sprzedam-i-kupie/warszawa/v1c9073l3200008p2";
            newUrl = "www.test.com";
            uri = "api/scanpage";
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
            var response = await Client.PostAsync(uri, payload);
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);

            var pages = await GetAllAsync<ScanPageDto>(uri);
            Assert.NotEmpty(pages);

            ScanPageDto myPage = pages.FirstOrDefault(x => x.UrlAddress == urlAddress);
            Assert.NotEqual(myPage, null);

            //update
            myPage.Active = false;
            myPage.UrlAddress = newUrl;

            payload = GetPayload(myPage);
            response = await Client.PutAsync(uri, payload);
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);

            // get by id
            response = await Client.GetAsync($"{uri}/{myPage.Id}");
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ScanPageDto>(responseString);
            result.UrlAddress.ShouldBeEquivalentTo(newUrl);

            // delete
            response = await Client.DeleteAsync($"{uri}/{result.Id}");
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);

            // get not found
            var responseNotFound = await Client.GetAsync($"{uri}/{myPage.Id}");
            var responseStringNotFound = await response.Content.ReadAsStringAsync();
            var resultNotFound = JsonConvert.DeserializeObject<ScanPageDto>(responseStringNotFound);
            resultNotFound.ShouldBeEquivalentTo(null);
        }

        /*
        [Fact, TestPriority(2)]
        public async Task change_scanpage_and_get_by_id()
        {
            var pages = await GetAllAsync<ScanPageDto>(uri);
            var page = pages.First();
            page.Active = false;
            page.UrlAddress = newUrl;

            var payload = GetPayload(page);
            var response = await Client.PutAsync(uri, payload);
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);

            var result = await GetAllAsync<ScanPageDto>(uri);
            ScanPageDto newScanPage = result.FirstOrDefault(x => x.UrlAddress == newUrl);
            newScanPage.ShouldBeEquivalentTo(page);
        }
        */
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
            response = await Client.PostAsync(uri, payload);
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);

            var pages = await GetAllAsync<ScanPageDto>(uri);
            Assert.NotEmpty(pages);

            Guid id = pages.FirstOrDefault().Id;
            response = await Client.DeleteAsync($"{uri}/{id}");
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);
        }
    }
}