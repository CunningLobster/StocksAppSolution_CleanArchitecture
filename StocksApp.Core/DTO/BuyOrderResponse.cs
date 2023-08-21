using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class BuyOrderResponse : IOrderResponse
    {
        public Guid BuyOrderID { get; set; }
        public string? StockSymbol { get; set; }
        public string? StockName { get; set; }
        public DateTime DateAndTimeOfOrder { get; set; }
        public uint Quantity { get; set; }
        public double Price { get; set; }
        public double TradeAmount { get; set; }
        public OrderType OrderType { get; set; } = OrderType.BuyOrder;

        public override string ToString()
        {
            return $"BuyOrderID: {BuyOrderID.ToString()}, StockSymbol: {StockSymbol}, StockName: {StockName}, DateAndTimeOfOrder: {DateAndTimeOfOrder.ToString("d")}, Quantity: {Quantity}, Price: {Price}, TradeAmount: {TradeAmount}, OrderType: {OrderType}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(BuyOrderResponse)) return false;

            BuyOrderResponse? other = obj as BuyOrderResponse;

            return BuyOrderID == other?.BuyOrderID
                   && StockSymbol == other.StockSymbol
                   && StockName == other.StockName
                   && DateAndTimeOfOrder == other.DateAndTimeOfOrder
                   && Quantity == other.Quantity
                   && Price == other.Price
                   && TradeAmount == other.TradeAmount
                   && OrderType == other.OrderType;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public static class BuyOrderResponseExtensions
    {
        /// <summary>
        /// An extension method which converts BuyOrder to BuyOrderResponse object
        /// </summary>
        /// <param name="buyOrder">Buy order object</param>
        /// <returns>BuyOrderResponse object based on the given BuyOrder</returns>
        public static BuyOrderResponse ToBuyOrderResponse(this BuyOrder buyOrder)
        {
            return new BuyOrderResponse { BuyOrderID = buyOrder.BuyOrderID, DateAndTimeOfOrder = buyOrder.DateAndTimeOfOrder, Price = buyOrder.Price, Quantity = buyOrder.Quantity, StockName = buyOrder.StockName, StockSymbol = buyOrder.StockSymbol, TradeAmount = buyOrder.Quantity * buyOrder.Price, OrderType = OrderType.BuyOrder };
        }
    }
}
