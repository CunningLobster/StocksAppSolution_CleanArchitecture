using AutoFixture;
using Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using ServiceContracts;
using ServiceContracts.DTO;
using StocksApp;
using StocksApp.Controllers;
using StocksApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksAppTests.ControllerTests
{
    public class TradeControllerTest
    {
        private readonly IFinnhubService _finnhubService;
        private readonly IStockBuyService _stockBuyService;
        private readonly IStockSellService _stockSellService;

        private readonly Mock<IFinnhubService> _finhubServiceMock;
        private readonly Mock<IStockBuyService> _stockBuyServiceMock;
        private readonly Mock<IStockSellService> _stockSellServiceMock;

        private readonly IOptions<TradingOptions> _tradingOptions;

        private readonly IFixture _fixture;

        private readonly ILogger<TradeController> _logger;
        private readonly Mock<ILogger<TradeController>> _loggerMock;

        public TradeControllerTest() 
        {
            _finhubServiceMock = new Mock<IFinnhubService>();
            _stockBuyServiceMock = new Mock<IStockBuyService>();
            _stockSellServiceMock = new Mock<IStockSellService>();

            _finnhubService = _finhubServiceMock.Object;
            _stockBuyService = _stockBuyServiceMock.Object;
            _stockSellService = _stockSellServiceMock.Object;

            _fixture = new Fixture();

            _tradingOptions = Options.Create(new TradingOptions() { DefaultOrderQuantity = _fixture.Create<int>(), DefaultStockSymbol = _fixture.Create<string>() });

            _loggerMock = new Mock<ILogger<TradeController>>();
            _logger = _loggerMock.Object;
        }

        #region Index

        [Fact]
        public async Task Index_ToBeSuccessful_ShouldReturnIndexView()
        {
            //Arrange
            Dictionary<string, object> companyProfile = new Dictionary<string, object>() 
            {
                {"ticker",_fixture.Create<string>()},
                {"name", _fixture.Create<string>() }
            };
            Dictionary<string, object> stockPriceQuote = new Dictionary<string, object>() 
            {
                {"c", _fixture.Create<double>()},
            };
            StockTrade stockTradeExpected = new StockTrade
            {
                StockSymbol = Convert.ToString(companyProfile["ticker"]),
                StockName = Convert.ToString(companyProfile["name"]),
                Price = Convert.ToDouble(Convert.ToString(stockPriceQuote["c"]), CultureInfo.InvariantCulture)
            };

            TradeController tradeController = new TradeController(_finnhubService, _tradingOptions, new ConfigurationBuilder().Build(), _stockBuyService, _stockSellService, _logger);

            _finhubServiceMock.Setup(f => f.GetCompanyProfile(It.IsAny<string>())).ReturnsAsync(companyProfile);
            _finhubServiceMock.Setup(f => f.GetStockPriceQuote(It.IsAny<string>())).ReturnsAsync(stockPriceQuote);

            //Act
            IActionResult result = await tradeController.Index(_fixture.Create<string>());

            //Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            viewResult.Model.Should().BeAssignableTo(typeof(StockTrade));
            viewResult.Model.Should().BeEquivalentTo(stockTradeExpected);
        }

        #endregion

        #region BuyOrder

        [Fact]
        public async Task BuyOrder_ModelStateIsValid_ShouldRedirectToOrdersView()
        {
            //Arrange
            BuyOrderRequest buyOrderRequest = _fixture.Create<BuyOrderRequest>();
            BuyOrderResponse buyOrderResponse = _fixture.Create<BuyOrderResponse>();

            TradeController tradeController = new TradeController(_finnhubService, _tradingOptions, new ConfigurationBuilder().Build(), _stockBuyService, _stockSellService, _logger);

            _stockBuyServiceMock.Setup(s => s.CreateBuyOrder(It.IsAny<BuyOrderRequest>())).ReturnsAsync(buyOrderResponse);

            //Act
            StockTrade stockTrade = new StockTrade
            {
                StockSymbol = Convert.ToString(buyOrderRequest.StockSymbol),
                StockName = Convert.ToString(buyOrderRequest.StockName),
                Price = Convert.ToDouble((buyOrderRequest.Price), CultureInfo.InvariantCulture)
            };

            IActionResult result = await tradeController.BuyOrder(buyOrderRequest);

            //Assert
            RedirectToActionResult viewResult = Assert.IsType<RedirectToActionResult>(result);
            viewResult.ActionName.Should().Be("Orders");
        }


        #endregion

        #region SellOrder

        [Fact]
        public async Task SellOrder_ModelStateIsValid_ShouldRedirectToOrdersView()
        {
            //Arrange
            SellOrderRequest sellOrderRequest = _fixture.Create<SellOrderRequest>();
            SellOrderResponse sellOrderResponse = _fixture.Create<SellOrderResponse>();

            TradeController tradeController = new TradeController(_finnhubService, _tradingOptions, new ConfigurationBuilder().Build(), _stockBuyService, _stockSellService, _logger);

            _stockSellServiceMock.Setup(s => s.CreateSellOrder(It.IsAny<SellOrderRequest>())).ReturnsAsync(sellOrderResponse);

            //Act
            StockTrade stockTrade = new StockTrade
            {
                StockSymbol = Convert.ToString(sellOrderRequest.StockSymbol),
                StockName = Convert.ToString(sellOrderRequest.StockName),
                Price = Convert.ToDouble((sellOrderRequest.Price), CultureInfo.InvariantCulture)
            };

            IActionResult result = await tradeController.SellOrder(sellOrderRequest);

            //Assert
            RedirectToActionResult viewResult = Assert.IsType<RedirectToActionResult>(result);
            viewResult.ActionName.Should().Be("Orders");
        }


        #endregion

        #region Orders

        [Fact]
        public async Task Orders_ToBeSuccessful_ReturnOrdersView()
        {
            //Arrange
            List<BuyOrder> buyOrders = _fixture.CreateMany<BuyOrder>().ToList();
            List<SellOrder> sellOrders = _fixture.CreateMany<SellOrder>().ToList();

            List<BuyOrderResponse> buyOrderResponses = buyOrders.Select(b => b.ToBuyOrderResponse()).ToList();
            List<SellOrderResponse> sellOrderResponses = sellOrders.Select(s => s.ToSellOrderResponse()).ToList();

            Orders ordersExpected = new Orders
            {
                BuyOrders = buyOrderResponses,
                SellOrders = sellOrderResponses
            };

            _stockBuyServiceMock.Setup(s => s.GetBuyOrders()).ReturnsAsync(buyOrderResponses);
            _stockSellServiceMock.Setup(s => s.GetSellOrders()).ReturnsAsync(sellOrderResponses);

            TradeController tradeController = new TradeController(_finnhubService, _tradingOptions, new ConfigurationBuilder().Build(), _stockBuyService, _stockSellService, _logger);

            //Act
            IActionResult result = await tradeController.Orders();

            //Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            viewResult.Model.Should().BeEquivalentTo(ordersExpected);
            viewResult.ViewName = "Orders";

        }

        #endregion

    }
}
