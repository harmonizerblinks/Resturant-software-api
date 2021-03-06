﻿using System;
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
        public int NominalId { get; set; }
        [Required]
        public string Id { get; set; }

        public AppUser AppUser { get; set; }
        public Nominal Nominal { get; set; }
        public IList<Transaction> Transactions { get; set;}
    }
}
