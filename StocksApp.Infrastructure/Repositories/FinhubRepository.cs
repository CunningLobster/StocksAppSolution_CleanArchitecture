using Microsoft.Extensions.Configuration;
using RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Repositories
{
    public class FinhubRepository : IFinhubRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public FinhubRepository(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }


        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            return await GetHttpResponse($"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}&token={_configuration["FINNHUB_API_KEY"]}");
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            return await GetHttpResponse($"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={_configuration["FINNHUB_API_KEY"]}");
        }

        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://finnhub.io/api/v1/stock/symbol?exchange=US&token={_configuration["FINNHUB_API_KEY"]}")
                };

                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
                string responseString = new StreamReader(httpResponseMessage.Content.ReadAsStream()).ReadToEnd();

                List<Dictionary<string, string>>? responseDict = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(responseString);

                if (responseDict == null)
                    throw new InvalidOperationException("No response from server");

                return responseDict;
            }
        }

        public async Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
        {
            return await GetHttpResponse($"https://finnhub.io/api/v1/search?q={stockSymbolToSearch}&token={_configuration["FINNHUB_API_KEY"]}");
        }

        async Task<Dictionary<string, object>> GetHttpResponse(string url)
        {
            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"{url}")
                };

                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
                string responseString = new StreamReader(httpResponseMessage.Content.ReadAsStream()).ReadToEnd();

                Dictionary<string, object>? responseDict = JsonSerializer.Deserialize<Dictionary<string, object>>(responseString);

                if (responseDict == null)
                    throw new InvalidOperationException("No response from server");
                if (responseDict.ContainsKey("error"))
                    throw new InvalidOperationException(Convert.ToString(responseDict["error"]));

                return responseDict;
            }

        }

    }
}
