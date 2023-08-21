namespace StocksApp.Models
{
    public class Stock
    {
        public string? StockSymbol { get; set; }
        public string? StockName { get; set; }
        public string? LogoUrl { get; set; }
        public string? FinnhubIndustry { get; set; }
        public string? Exchange { get; set; } 
        public double? Price { get; set; }

    }
}
