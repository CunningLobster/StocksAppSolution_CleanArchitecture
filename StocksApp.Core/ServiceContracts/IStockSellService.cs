using ServiceContracts.DTO;

namespace ServiceContracts
{
    public interface IStockSellService
    {
        /// <summary>
        /// Create a sell order based on the given SellOrderRequest
        /// </summary>
        /// <param name="sellOrderRequest">Given Sell order request</param>
        /// <returns>Sell order as an object of SellOrderResponse</returns>
        Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest);
        /// <summary>
        /// Get a list of Sell Orders as list of SellOrderResponse objects
        /// </summary>
        /// <returns>List of SellOrderResponse objects</returns>
        Task<List<SellOrderResponse>> GetSellOrders();
    }
}
