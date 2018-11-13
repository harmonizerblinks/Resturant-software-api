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
        public string Quantity { get; set; }
    }
}
