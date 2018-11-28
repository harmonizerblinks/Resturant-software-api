using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Resturant.Models
{
    public class Item : BaseModel
    {
        [Key]
        public int ItemId { get; set; }
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public decimal SalePrice { get; set; }

        public IList<Stock> Stock { get; set; }
        public IList<StockLog> Logs { get; set; }
    }
}
