using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class SellOrderResponse : IOrderResponse
    {
        public Guid SellOrderID { get; set; }
        public string? StockSymbol { get; set; }
        public string? StockName { get; set; }
        public DateTime DateAndTimeOfOrder { get; set; }
        public uint Quantity { get; set; }
        public double Price { get; set; }
        public double TradeAmount { get; set; }
        public OrderType OrderType { get; set; } = OrderType.SellOrder;

        public override string ToString()
        {
            return $"SellOrderID: {SellOrderID.ToString()}, StockSymbol: {StockSymbol}, StockName: {StockName}, DateAndTimeOfOrder: {DateAndTimeOfOrder.ToString("d")}, Quantity: {Quantity}, Price: {Price}, TradeAmount: {TradeAmount}, OrderType: {OrderType}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(SellOrderResponse)) return false;

            SellOrderResponse? other = obj as SellOrderResponse;

            return SellOrderID == other?.SellOrderID
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

    public static class SellOrderExtensions
    {
        public static SellOrderResponse ToSellOrderResponse(this SellOrder sellOrder)
        {
            return new SellOrderResponse { SellOrderID = sellOrder.SellOrderID, StockSymbol = sellOrder.StockSymbol, StockName = sellOrder.StockName, DateAndTimeOfOrder = sellOrder.DateAndTimeOfOrder, Price = sellOrder.Price, Quantity = sellOrder.Quantity, TradeAmount = sellOrder.Price * sellOrder.Quantity, OrderType = OrderType.SellOrder };
        }
    }
}
