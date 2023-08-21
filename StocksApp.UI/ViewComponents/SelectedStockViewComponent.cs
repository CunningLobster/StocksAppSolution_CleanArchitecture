using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts;
using StocksApp.Models;

namespace StocksApp.ViewComponents
{
    public class SelectedStockViewComponent : ViewComponent
    {
        private readonly IFinnhubService _finnhubService;

        public SelectedStockViewComponent(IFinnhubService finnhubService)
        {
            _finnhubService = finnhubService;
        }


        public async Task<IViewComponentResult> InvokeAsync(string stockSymbol)
        {
            Dictionary<string, object>? companyDetails = await _finnhubService.GetCompanyProfile(stockSymbol);
            Dictionary<string, object>? stockPriceQuote = await _finnhubService.GetStockPriceQuote(stockSymbol);

            Stock stockProfile = new Stock()
            {
                StockSymbol = companyDetails["ticker"].ToString(),
                StockName = companyDetails["name"].ToString(),
                LogoUrl = companyDetails["logo"].ToString(),
                Exchange = companyDetails["exchange"].ToString(),
                FinnhubIndustry = companyDetails["finnhubIndustry"].ToString(),
                Price = Convert.ToDouble(stockPriceQuote["c"].ToString(), System.Globalization.CultureInfo.InvariantCulture)
            };

            return View(stockProfile);
        }
    }
}
