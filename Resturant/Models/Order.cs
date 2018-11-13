using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Resturant.Models
{
    public class Order : BaseModel
    {
        [Key]
        public int OrderId { get; set; }
        public string OrderNo { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Mobile { get; set; }
        [Required]
        public int LocationId { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Status { get; set; }

        public Location Location { get; set; }
    }
}
