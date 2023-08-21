using ServiceContracts.CustomValidators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public interface IOrderRequest
    {
        [Required]
        public string? StockSymbol { get; set; }
        [Required]
        public string? StockName { get; set; }
        [MinimumDateValidation("2000-01-01")]
        public DateTime DateAndTimeOfOrder { get; set; }
        [Range(1, 100000, ConvertValueInInvariantCulture = true)]
        public uint Quantity { get; set; }
        [Range(1, 10000, ConvertValueInInvariantCulture = true)]
        public double Price { get; set; }
    }
}
