using Fizzler.Systems.HtmlAgilityPack;
using FluentAssertions;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksAppTests.IntegrationTests
{
    public class StocksControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public StocksControllerIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        #region Explore

        [Fact]
        public async Task Explore_ToReturnView()
        {
            //Act
            HttpResponseMessage response = await _client.GetAsync("/Stocks/Explore");

            //Assert
            response.Should().BeSuccessful();

            string responseBody = await response.Content.ReadAsStringAsync();

            HtmlDocument html = new HtmlDocument();
            html.LoadHtml(responseBody);

            HtmlNode document = html.DocumentNode;

            document.QuerySelectorAll(".stocks-list").Should().NotBeNull();
        }

        #endregion
    }
}
