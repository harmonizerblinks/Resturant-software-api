using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Resturant.Models
{
    public class Sales : BaseModel
    {
        [Key]
        public int SalesId { get; set; }
        public int ItemId { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public int Quantity { get; set; }

        public Item Item { get; set; }
        public Order Order { get; set; }
    }
}
