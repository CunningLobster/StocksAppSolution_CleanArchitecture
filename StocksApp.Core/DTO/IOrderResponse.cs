using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public interface IOrderResponse
    {
        public string? StockSymbol { get; set; }
        public string? StockName { get; set; }
        public DateTime DateAndTimeOfOrder { get; set; }
        public uint Quantity { get; set; }
        public double Price { get; set; }
        public double TradeAmount { get; set; }

        public OrderType OrderType { get; set; }
    }

    public enum OrderType
    {
        [Display(Name = "Buy Order")]
        BuyOrder,
        [Display(Name = "Sell Order")]
        SellOrder
    }
}
