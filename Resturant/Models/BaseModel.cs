using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Resturant.Models
{
    public class BaseModel
    {
        [Required]
        public int UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
