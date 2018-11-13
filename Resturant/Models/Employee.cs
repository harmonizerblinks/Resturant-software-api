using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Resturant.Models
{
    public class Employee : BaseModel
    {
        [Key]
        public int EmployeeId { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public string Mobile { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string Position { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string Nationality { get; set; }
        [Required]
        public string Address { get; set; }
    }
}
