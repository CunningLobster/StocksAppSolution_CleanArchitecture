using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using ServiceContracts;
using ServiceContracts.DTO;
using StocksApp;
using StocksApp.Controllers;
using StocksApp.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksAppTests.FilterTests
{
    public class ActionFilterTests
    {
        #region CreateOrderActionFilter
        private readonly IFinnhubService _finnhubService;
        private readonly IStockBuyService _stockBuyService;
        private readonly IStockSellService _stockSellService;

        private readonly Mock<IFinnhubService> _finhubServiceMock;
        private readonly Mock<IStockBuyService> _stockBuyServiceMock;
        private readonly Mock<IStockSellService> _stockSellServiceMock;

        private readonly IOptions<TradingOptions> _tradingOptions;

        private readonly ILogger<TradeController> _logger;
        private readonly Mock<ILogger<TradeController>> _loggerMock;

        private readonly IFixture _fixture;

        private readonly ActionExecutionDelegate _next;
        private readonly Mock<ActionExecutionDelegate> _nextMock;
        public ActionFilterTests()
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

            _nextMock = new Mock<ActionExecutionDelegate>();
            _next = _nextMock.Object;
        }

        [Fact]
        public async Task OnActionExecutionAsync_ModelStateIsInvalid_ResultIsIndexView()
        {
            //Arrange
            IOrderRequest orderRequest = _fixture.Build<BuyOrderRequest>().With(o => o.Quantity, (uint)0).Create();

            //Action context
            ActionContext actionContext = new ActionContext() {
                HttpContext = new DefaultHttpContext(),
                RouteData = new Microsoft.AspNetCore.Routing.RouteData(),
                ActionDescriptor = new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor(),
            };
            //Filters metadata
            CreateOrderActionFilter filter = new CreateOrderActionFilter();
            List<IFilterMetadata> filters = new List<IFilterMetadata> { filter };
            //Action arguments
            Dictionary<string, object> arguments = new Dictionary<string, object>();
            arguments["orderRequest"] = orderRequest;
            //Controller
            TradeController tradeController = new TradeController(_finnhubService, _tradingOptions, new ConfigurationBuilder().Build(), _stockBuyService, _stockSellService, _logger);

            ActionExecutingContext context = new ActionExecutingContext(actionContext,
                filters, arguments, tradeController);

            //Act
            await filter.OnActionExecutionAsync(context, _next);

            IActionResult? result = context.Result;

            //Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            viewResult.ViewName = "Index";
        }

        #endregion
    }
}
