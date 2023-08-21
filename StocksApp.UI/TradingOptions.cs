namespace StocksApp
{
    public class TradingOptions
    {
        public const string TradingOptionsSection = "TradingOptions";
        public string? DefaultStockSymbol { get; set; }
        public string? SelectedStockSymbol { get; set; }
        public int? DefaultOrderQuantity { get; set; }
        public string? Top25PopularStocks { get; set; }
    }
}
