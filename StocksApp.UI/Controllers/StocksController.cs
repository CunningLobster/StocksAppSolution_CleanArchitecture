using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ServiceContracts;
using StocksApp.Models;

namespace StocksApp.Controllers
{
    public class StocksController : Controller
    {
        private readonly IFinnhubService _finnhubService;
        private readonly IOptions<TradingOptions> _options;
        private readonly ILogger<StocksController> _logger;

        public StocksController(IFinnhubService finnhubService, IOptions<TradingOptions> options, ILogger<StocksController> logger)
        {
            _finnhubService = finnhubService;
            _options = options;
            _logger = logger;
        }

        [Route("/")]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Explore(bool showAll = false)
        {
            _logger.LogInformation("Explore Method from StocksControler");

            if (_options.Value.SelectedStockSymbol == null)
                _options.Value.SelectedStockSymbol = _options.Value.DefaultStockSymbol;
            ViewBag.Options = _options.Value;

            List<Dictionary<string, string>>? allStocks = await _finnhubService.GetStocks();
            List<string>? top25stocks = _options.Value.Top25PopularStocks!.Split(',').ToList();

            List<Stock> stocks = new List<Stock>();

            if (showAll == true)
            {
                foreach (var stock in allStocks)
                {
                    stocks.Add(new Stock
                    {
                        StockSymbol = stock["symbol"],
                        StockName = stock["description"]
                    });
                }
            }
            else
            {
                foreach (string stockFromTop in top25stocks)
                {
                    Dictionary<string, string>? stockToAdd = allStocks.FirstOrDefault(s => s["symbol"] == stockFromTop);

                    stocks.Add(new Stock
                    {
                        StockSymbol = stockToAdd["symbol"],
                        StockName = stockToAdd["description"]
                    });
                }
            }

            return View(stocks);
        }


        [Route("selected-stock/{stock?}")]
        [Route("[controller]/selected-stock/{stock?}")]
        [Route("[controller]/Explore/selected-stock/{stock?}")]
        public async Task<IActionResult> ShowSelectedStock(string? stock)
        {
            _logger.LogInformation("ShowSelectedStock Method from StocksControler");

            if (string.IsNullOrEmpty(stock))
                stock = _options.Value.DefaultStockSymbol;
            _options.Value.SelectedStockSymbol = stock;

            ViewBag.Options = _options.Value;

            return ViewComponent("SelectedStock", new { stockSymbol = stock });
        }
    }
}
