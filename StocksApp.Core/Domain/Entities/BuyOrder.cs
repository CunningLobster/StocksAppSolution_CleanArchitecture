using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class BuyOrder
    {
        [Key]
        public Guid BuyOrderID { get; set; }
        [Required]
        [StringLength(10)]
        public string? StockSymbol { get; set; }
        [Required]
        [StringLength(50)]
        public string? StockName { get; set; }
        public DateTime DateAndTimeOfOrder { get; set; }
        [Range(1, 100000, ConvertValueInInvariantCulture = true)]
        public uint Quantity { get; set; }
        [Range(1, 10000, ConvertValueInInvariantCulture = true)]
        public double Price { get; set; }
    }
}