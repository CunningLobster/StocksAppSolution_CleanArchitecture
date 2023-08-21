using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
using StocksApp.Filters;
using StocksApp.Models;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics;
using System.Globalization;

namespace StocksApp.Controllers
{
    [Route("[controller]")]
    public class TradeController : Controller
    {
        private readonly IFinnhubService _finnhubService;
        private readonly IStockBuyService _stockBuyService;
        private readonly IStockSellService _stockSellService;

        private readonly IOptions<TradingOptions> _options;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TradeController> _logger;

        public TradeController(IFinnhubService finnhubService, IOptions<TradingOptions> options, IConfiguration configuration, IStockBuyService stockBuyService, IStockSellService stockSellService, ILogger<TradeController> logger)
        {
            _finnhubService = finnhubService;
            _options = options;
            _configuration = configuration;
            _stockBuyService = stockBuyService;
            _stockSellService = stockSellService;
            _logger = logger;
        }

        [Route("[action]/{stockSymbol?}")]
        public async Task<IActionResult> Index(string? stockSymbol)
        {
            _logger.LogInformation("Index Method from TradeController");
            _logger.LogDebug($"StockSymbol: {stockSymbol}");

            if (string.IsNullOrEmpty(_options.Value.DefaultStockSymbol))
                _configuration.GetSection(TradingOptions.TradingOptionsSection).Bind(_options);
            if (string.IsNullOrEmpty(stockSymbol))
                stockSymbol = _options.Value.DefaultStockSymbol!;
            _options.Value.SelectedStockSymbol = stockSymbol;

            ViewBag.Options = _options.Value;

            var companyProfile = await _finnhubService.GetCompanyProfile(stockSymbol);
            var stockPriceQuote = await _finnhubService.GetStockPriceQuote(stockSymbol);

            StockTrade stockTrade = new StockTrade {
                StockSymbol = Convert.ToString(companyProfile["ticker"]),
                StockName = Convert.ToString(companyProfile["name"]),
                Price = Convert.ToDouble(Convert.ToString(stockPriceQuote["c"]), CultureInfo.InvariantCulture)
            };

            ViewBag.FinnhubApiKey = _configuration["FINNHUB_API_KEY"];
            ViewBag.DefaultOrderQuantity = _options.Value.DefaultOrderQuantity;

            return View(stockTrade);
        }

        [Route("[action]")]
        public IActionResult RedirectToIndex()
        {
            _logger.LogInformation("RedirectToIndex Method from TradeController");

            return RedirectToAction(nameof(Index), new { StockSymbol = _options.Value.SelectedStockSymbol });
        }

        [Route("[action]")]
        [HttpPost]
        [TypeFilter(typeof(CreateOrderActionFilter))]
        public async Task<IActionResult> BuyOrder(BuyOrderRequest orderRequest)
        {
            _logger.LogInformation("BuyOrder Method from TradeController");
            await _stockBuyService.CreateBuyOrder(orderRequest);

            return RedirectToAction(nameof(Orders));
        }

        [Route("[action]")]
        [HttpPost]
        [TypeFilter(typeof(CreateOrderActionFilter))]
        public async Task<IActionResult> SellOrder(SellOrderRequest orderRequest)
        {
            _logger.LogInformation("SellOrder Method from TradeController");
            await _stockSellService.CreateSellOrder(orderRequest);

            return RedirectToAction(nameof(Orders));
        }


        [Route("[action]")]
        public async Task<IActionResult> Orders() 
        {
            _logger.LogInformation("Orders Method from TradeController");

            Orders orders = new Orders();
            orders.BuyOrders = await _stockBuyService.GetBuyOrders();
            orders.SellOrders = await _stockSellService.GetSellOrders();

            ViewBag.Options = _options.Value;
            return View(orders);
        }

        public async Task<IActionResult> OrdersPDF()
        {
            List<IOrderResponse> orders = new List<IOrderResponse>();
            orders.AddRange(await _stockBuyService.GetBuyOrders());
            orders.AddRange(await _stockSellService.GetSellOrders());

            return new ViewAsPdf("OrdersPDF", orders) {
                FileName = "stocksOrders.pdf",
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                PageMargins = new Rotativa.AspNetCore.Options.Margins(20,20,20,20)
            };
        }
    }
}
