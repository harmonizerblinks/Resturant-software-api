using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }
        [Required]
        public string Method { get; set; }
        [Required]
        public int NominalId { get; set; }
        public int? TellerId { get; set; }
        [NotMapped]
        public int Id { get; set; }
        [Required]
        public string Reference { get; set; }

        public Nominal Nominal { get; set; }
        public Teller Teller { get; set; }
    }
}
