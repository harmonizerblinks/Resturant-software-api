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
        //[DataType(DataType.Currency)]
        public decimal Price { get; set; }
        [Required]
        //[DataType(DataType.Currency)]
        public decimal Amount { get; set; }
        public string Type { get; set; }
        [Required]
        public int Quantity { get; set; }
        public string Reference { get; set; }

        public Item Item { get; set; }
    }
}
