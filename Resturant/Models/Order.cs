using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Resturant.Models
{
    public class Order : BaseModel
    {
        [Key]
        public int OrderId { get; set; }
        public string OrderNo { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Mobile { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public int LocationId { get; set; }
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        [DataType(DataType.Currency)]
        public decimal Vat { get; set; }
        [DataType(DataType.Currency)]
        public decimal Total { get; set; }
        public string Source { get; set; }
        [Required]
        public string Method { get; set; }
        public string Address { get; set; }
        [Required]
        public string Status { get; set; }
        public int TransactionId { get; set; }

        public Location Location { get; set; }
        public Transaction Transaction { get; set; }
        public IList<OrderList> Orderlist { get; set; }
    }
}
