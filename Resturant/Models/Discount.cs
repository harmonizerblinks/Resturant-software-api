using System;
using System.ComponentModel.DataAnnotations;

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

        public Location Location { get; set; }
    }
}
