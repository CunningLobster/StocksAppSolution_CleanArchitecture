using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ServiceContracts.CustomValidators;
using Entities;

namespace ServiceContracts.DTO
{
    public class BuyOrderRequest : IOrderRequest
    {
        [Required]
        public string? StockSymbol { get; set; }
        [Required]
        public string? StockName { get; set; }
        [MinimumDateValidation("2000-01-01")]
        [DataType(DataType.DateTime)]
        public DateTime DateAndTimeOfOrder { get; set; }
        [Range(1, 100000, ConvertValueInInvariantCulture = true)]
        public uint Quantity { get; set; }
        [Range(1, 10000, ConvertValueInInvariantCulture = true)]
        public double Price { get; set; }

        public BuyOrder ToBuyOrder() 
        {
            return new BuyOrder() { DateAndTimeOfOrder = DateAndTimeOfOrder, Price = Price, Quantity = Quantity, StockName = StockName, StockSymbol = StockSymbol };
        }
    }
}
