using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Resturant.Models
{
    public class Stock : BaseModel
    {
        [Key]
        public int StockId { get; set; }
        [Required]
        public int ItemId { get; set; }
        [Required]
        public int Quantity { get; set; }

        public Item Item { get; set; }

        public IList<StockLog> Logs { get; set; }
    }
}
