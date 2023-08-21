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
    public class TradeControllerIntagrationTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public TradeControllerIntagrationTest(CustomWebApplicationFactory factory)
        { 
            _client = factory.CreateClient();
        }

        #region Index

        [Fact]
        public async Task Index_ToReturnView()
        {
            //Act
            HttpResponseMessage response = await _client.GetAsync("/Trade/Index");

            //Assert
            response.Should().BeSuccessful();

            string responseBody = await response.Content.ReadAsStringAsync();

            HtmlDocument html = new HtmlDocument();
            html.LoadHtml(responseBody);

            HtmlNode document = html.DocumentNode;

            document.QuerySelectorAll(".price").Should().NotBeNull();
        }

        #endregion
    }
}
