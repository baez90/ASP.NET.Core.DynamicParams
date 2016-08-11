using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ASP.NET.Core.DynamicParams.TestApp;
using ASP.NET.Core.DynamicParams.TestApp.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Xunit;

namespace ASP.NET.Core.DynamicParams.Tests
{
    public class DynamicFilterTest
    {
        public DynamicFilterTest()
        {
            var server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            _client = server.CreateClient();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("testresources.json")
                .Build();
        }

        private readonly IConfigurationRoot _config;
        private readonly HttpClient _client;

        [Fact]
        public async Task TestExtractPerson()
        {
            var response = await _client.PostAsync("/api/Values/extractPerson", new StringContent(_config["SingleUserJson"], Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var person = JsonConvert.DeserializeObject<Person>(responseString);

            Assert.Equal(person.FirstName, "Ted");
            Assert.Equal(person.LastName, "Tester");
        }

        [Fact]
        public async Task TestExtractPersonWithImplicitParamDetection()
        {
            var response = await _client.PostAsync("/api/Values/extractPersonImplicit", new StringContent(_config["SingleUserJson"], Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var person = JsonConvert.DeserializeObject<Person>(responseString);

            Assert.Equal(person.FirstName, "Ted");
            Assert.Equal(person.LastName, "Tester");
        }

        [Fact]
        public async Task TestExtractUserFromCollection()
        {
            var response = await _client.PostAsync("/api/Values/collection", new StringContent(_config["UserCollectionJson"], Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var person = JsonConvert.DeserializeObject<Person>(responseString);

            Assert.Equal(person.FirstName, "Ted");
            Assert.Equal(person.LastName, "Tester");
        }

        [Fact]
        public async Task TestExtractUserFromCollectionWithImplicitParamDetection()
        {
            var response = await _client.PostAsync("/api/Values/collectionImplicit", new StringContent(_config["UserCollectionJson"], Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var person = JsonConvert.DeserializeObject<Person>(responseString);

            Assert.Equal(person.FirstName, "Ted");
            Assert.Equal(person.LastName, "Tester");
        }
    }
}