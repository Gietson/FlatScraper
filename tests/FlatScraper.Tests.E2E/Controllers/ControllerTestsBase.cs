using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FlatScraper.API;
using FlatScraper.Infrastructure.DTO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;

namespace FlatScraper.Tests.E2E.Controllers
{
    public class ControllerTestsBase
    {
        protected readonly HttpClient Client;
        protected readonly TestServer Server;

        protected ControllerTestsBase()
        {
            Server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            Client = Server.CreateClient();
        }

        protected static StringContent GetPayload(object data)
        {
            var json = JsonConvert.SerializeObject(data);

            return new StringContent(json, Encoding.UTF8, "application/json");
        }
        /*
        protected async Task<T> GetAsync<T, T1>(string uri, T1 value)
        {
            var response = await Client.GetAsync($"{uri}/{value}");
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<T>(responseString);
            return result;
        }*/

        protected async Task<IEnumerable<T>> GetAllAsync<T>(string uri)
        {
            var response = await Client.GetAsync(uri);
            var responseString = await response.Content.ReadAsStringAsync();

            var pages = JsonConvert.DeserializeObject<IEnumerable<T>>(responseString);
            return pages;
        }
    }
}