using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Resturant.Models
{
    public class Transaction : BaseModel
    {
        [Key]
        public int TransactionId { get; set; }
        public string TransCode { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Source { get; set; }
        [Required]
        public string Amount { get; set; }
        [Required]
        public string Method { get; set; }
        [Required]
        public int NominalId { get; set; }
        public int? TellerId { get; set; }
        public int? OrderId { get; set; }
        [Required]
        public string Reference { get; set; }

        public Nominal Nominal { get; set; }
        public Teller Teller { get; set; }
        public Order Order { get; set; }
    }
}
