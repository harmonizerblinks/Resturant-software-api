using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Resturant.Models
{
    public class Nominal : BaseModel
    {
        [Key]
        public int NominalId { get; set; }
        [Required]
        public string GLType { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string BalanceType { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Status { get; set; }

        [NotMapped]
        public decimal Balance { get; set; }

        public IList<Transaction> Transactions { get; set; }
    }
}
