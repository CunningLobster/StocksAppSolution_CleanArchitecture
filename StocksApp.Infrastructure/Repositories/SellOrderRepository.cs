using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace Repositories
{
    public class SellOrderRepository : ISellOrderRepository
    {
        private readonly ApplicationDbContext _db;

        public SellOrderRepository(ApplicationDbContext db) 
        {
            _db = db;
        }

        public async Task<SellOrder> AddSellOrder(SellOrder sellOrder)
        {
            _db.SellOrders.Add(sellOrder);
            await _db.SaveChangesAsync();
            return sellOrder;
        }

        public async Task<List<SellOrder>> GetSellOrders()
        {
            return await _db.SellOrders.ToListAsync();
        }
    }
}
