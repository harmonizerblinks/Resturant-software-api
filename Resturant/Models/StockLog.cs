using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Resturant.Models
{
    public class StockLog
    {
        [Key]
        public int StockLogId { get; set; }
        [Required]
        public int StockId { get; set; }
        [Required]
        public int ItemId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [NotMapped]
        public string Name { get; set; }
        [NotMapped]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public string UserId { get; set; }
        public DateTime Date { get; set; }

        public Item Item { get; set; }
        public Stock Stock { get; set; }
    }
}
