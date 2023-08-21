using Entities;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class StockSellService : IStockSellService
    {
        private readonly ISellOrderRepository _sellOrderRepository;

        public StockSellService(ISellOrderRepository sellOrderRepository)
        {
            _sellOrderRepository = sellOrderRepository;
        }

        public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            if (sellOrderRequest == null)
                throw new ArgumentNullException(nameof(sellOrderRequest));

            ValidationHelper.ModelValidation(sellOrderRequest);

            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            sellOrder.SellOrderID = Guid.NewGuid();

            await _sellOrderRepository.AddSellOrder(sellOrder);

            return sellOrder.ToSellOrderResponse();
        }

        public async Task<List<SellOrderResponse>> GetSellOrders()
        {
            List<SellOrder> sellOrders = await _sellOrderRepository.GetSellOrders();
            if (!sellOrders.Any())
                return new List<SellOrderResponse>();

            List<SellOrderResponse> sellOrderResponses = new List<SellOrderResponse>();
            foreach (var sellOrder in sellOrders)
            {
                SellOrderResponse sellOrderResponse = sellOrder.ToSellOrderResponse();
                sellOrderResponses.Add(sellOrderResponse);
            }

            return sellOrderResponses;
        }
    }
}
