using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ASP.NET.Core.DynamicParams.TestApp;
using ASP.NET.Core.DynamicParams.TestApp.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Xunit;

namespace ASP.NET.Core.DynamicParams.Tests
{
    public class DynamicFilterTest
    {
        public DynamicFilterTest()
        {
            _server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            _client = _server.CreateClient();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private readonly TestServer _server;
        private readonly HttpClient _client;

        [Fact]
        public async Task TestExtractPerson()
        {
            var response =
                await
                    _client.PostAsync("/api/Values/extractPerson",
                        new StringContent(
                            "{\r\n    \"User\": {\r\n        \"FirstName\": \"Ted\",\r\n        \"Lastname\": \"Tester\"\r\n    },\r\n    \"Title\": \"Prof.\"\r\n}",
                            Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var person = JsonConvert.DeserializeObject<Person>(responseString);

            Assert.Equal(person.FirstName, "Ted");
            Assert.Equal(person.LastName, "Tester");
        }

        [Fact]
        public async Task TestExtractUserFromCollection()
        {
            var response = await _client.PostAsync("/api/Values/collection",
                new StringContent(
                    "[\r\n    {\r\n        \"User\": {\r\n            \"FirstName\": \"Ted\",\r\n            \"Lastname\": \"Tester\"\r\n        },\r\n        \"Title\": \"Prof.\"\r\n    },\r\n    {\r\n        \"User\": {\r\n            \"FirstName\": \"Ted\",\r\n            \"Lastname\": \"Tester\"\r\n        },\r\n        \"Title\": \"Prof.\"\r\n    },\r\n    {\r\n        \"User\": {\r\n            \"FirstName\": \"Ted\",\r\n            \"Lastname\": \"Tester\"\r\n        },\r\n        \"Title\": \"Prof.\"\r\n    }\r\n]",
                    Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var person = JsonConvert.DeserializeObject<Person>(responseString);

            Assert.Equal(person.FirstName, "Ted");
            Assert.Equal(person.LastName, "Tester");
        }
    }
}