using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Resturant.Models
{
    public class OrderList
    {
        [Key]
        public int OrderListId { get; set; }
        public int OrderId { get; set; }
        [Required]
        public int FoodId { get; set; }
        [Required]
        //[DataType(DataType.Currency)]
        public decimal Amount { get; set; }
        [Required]
        public int Quantity { get; set; }

        public Food Food { get; set; }
        public Order Order { get; set; }
    }
}
