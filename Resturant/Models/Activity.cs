using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Resturant.Models
{
    public class Activity
    {
        [Key]
        public int ActivityId { get; set; }

        public string Action { get; set; }

        public string Claim { get; set; }

        public string Details { get; set; }

        public int UserId { get; set; }

        public DateTime DateTime { get; set; }
    }
}
