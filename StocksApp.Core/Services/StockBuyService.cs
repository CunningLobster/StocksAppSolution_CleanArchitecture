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
    public class StockBuyService : IStockBuyService
    {
        private readonly IBuyOrderRepository _buyOrderRepository;

        public StockBuyService(IBuyOrderRepository buyOrderRepository)
        {
            _buyOrderRepository = buyOrderRepository;
        }

        public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            if (buyOrderRequest == null)
                throw new ArgumentNullException(nameof(buyOrderRequest));

            ValidationHelper.ModelValidation(buyOrderRequest);

            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            buyOrder.BuyOrderID = Guid.NewGuid();

            await _buyOrderRepository.AddBuyOrder(buyOrder);

            return buyOrder.ToBuyOrderResponse();
        }

        public async Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            List<BuyOrder> buyOrders = await _buyOrderRepository.GetBuyOrders();
            if(!buyOrders.Any())
                return new List<BuyOrderResponse>();

            List<BuyOrderResponse> buyOrderResponses = new List<BuyOrderResponse>();
            foreach (var buyOrder in buyOrders)
            {
                BuyOrderResponse buyOrderResponse = buyOrder.ToBuyOrderResponse();
                buyOrderResponses.Add(buyOrderResponse);
            }

            return buyOrderResponses;
        }
    }
}
