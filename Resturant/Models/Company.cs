using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Resturant.Models
{
    public class Company : BaseModel
    {
        [Key]
        public int CompanyId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Mobile { get; set; }
        [Required]
        public string Postal { get; set; }
        [Required]
        public string Address { get; set; }
    }
}
