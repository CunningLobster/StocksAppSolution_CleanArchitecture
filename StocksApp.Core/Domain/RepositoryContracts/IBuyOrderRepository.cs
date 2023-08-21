using Entities;

namespace RepositoryContracts
{
    public interface IBuyOrderRepository
    {
        /// <summary>
        /// Add buy order to the data store
        /// </summary>
        /// <param name="buyOrder">Buy order to add</param>
        /// <returns>Buy order object after adding to the data store</returns>
        public Task<BuyOrder> AddBuyOrder(BuyOrder buyOrder);

        /// <summary>
        /// Get buy orders from the data store
        /// </summary>
        /// <returns>Buy order objects from the data store</returns>
        public Task<List<BuyOrder>> GetBuyOrders();
    }
}