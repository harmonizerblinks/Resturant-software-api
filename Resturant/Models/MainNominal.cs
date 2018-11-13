using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Resturant.Models
{
    public class MainNominal : BaseModel
    {
        [Key]
        public int LocationId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Status { get; set; }
    }
}
