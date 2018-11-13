using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Resturant.Models
{
    public class Teller : BaseModel
    {
        [Key]
        public int TellerId { get; set; }
        [Required]
        public int ItemId { get; set; }
        [Required]
        public int NominalId { get; set; }
        [Required]
        public string Id { get; set; }

        public AppUser User { get; set; }
    }
}
