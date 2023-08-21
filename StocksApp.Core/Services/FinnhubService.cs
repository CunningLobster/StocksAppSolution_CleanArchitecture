using Microsoft.Extensions.Configuration;
using RepositoryContracts;
using ServiceContracts;
using System.Net.Http;
using System.Text.Json;

namespace Services
{
    public class FinnhubService : IFinnhubService
    {
        private readonly IFinhubRepository _finhubRepository;

        public FinnhubService(IFinhubRepository finhubRepository)
        {
            _finhubRepository = finhubRepository;
        }

        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            return await _finhubRepository.GetCompanyProfile(stockSymbol);
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            return await _finhubRepository.GetStockPriceQuote(stockSymbol);
        }

        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
            return await _finhubRepository.GetStocks();
        }

        public async Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
        {
            return await _finhubRepository.SearchStocks(stockSymbolToSearch);
        }
    }
}