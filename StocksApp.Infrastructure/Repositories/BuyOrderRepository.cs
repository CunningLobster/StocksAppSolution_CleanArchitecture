using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace Repositories
{
    public class BuyOrderRepository : IBuyOrderRepository
    {
        private readonly ApplicationDbContext _db;

        public BuyOrderRepository(ApplicationDbContext db)
        { 
            _db = db;
        }

        public async Task<BuyOrder> AddBuyOrder(BuyOrder buyOrder)
        {
            _db.Add(buyOrder);
            await _db.SaveChangesAsync();
            return buyOrder;
        }

        public async Task<List<BuyOrder>> GetBuyOrders()
        {
            return await _db.BuyOrders.ToListAsync();
        }
    }
}