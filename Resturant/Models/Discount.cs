using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Resturant.Models
{
    public class Discount : BaseModel
    {
        [Key]
        public int DiscountId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Mobile { get; set; }
        [Required]
        public int Percent { get; set; }
        public int LocationId { get; set; }
    }
}
