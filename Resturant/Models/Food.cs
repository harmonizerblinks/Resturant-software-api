using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Resturant.Models
{
    public class Food : BaseModel
    {
        [Key]
        public int FoodId { get; set; }
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Price { get; set; }
        [Required]
        public DateTime Discount { get; set; }
        [Required]
        public int NominalId { get; set; }
    }
}
