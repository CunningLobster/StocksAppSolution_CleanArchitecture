using ServiceContracts.DTO;

namespace ServiceContracts
{
    public interface IStockBuyService
    {
        /// <summary>
        /// Create a buy order based on the given BuyOrderRequest
        /// </summary>
        /// <param name="buyOrderRequest">Given Buy order request</param>
        /// <returns>Buy order as an object of BuyOrderResponse</returns>
        Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest);
        /// <summary>
        /// Get a list of Buy Orders as list of BuyOrderResponse objects
        /// </summary>
        /// <returns>List of BuyOrderResponse objects</returns>
        Task<List<BuyOrderResponse>> GetBuyOrders();
    }
}
