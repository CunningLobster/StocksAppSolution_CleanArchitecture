using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryContracts
{
    public interface ISellOrderRepository
    {
        /// <summary>
        /// Add sell order to the data store
        /// </summary>
        /// <param name="sellOrder">Sell order to add</param>
        /// <returns>Sell order object after adding to the data store</returns>
        public Task<SellOrder> AddSellOrder(SellOrder sellOrder);

        /// <summary>
        /// Get sell orders from the data store
        /// </summary>
        /// <returns>Sell order objects from the data store</returns>
        public Task<List<SellOrder>> GetSellOrders();

    }
}
