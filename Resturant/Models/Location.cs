using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Resturant.Models
{
    public class Location : BaseModel
    {
        [Key]
        public int LocationId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Discount { get; set; }

        public IList<Discount> Discounts { get; set; }
    }
}
