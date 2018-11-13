using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Resturant.Models
{
    public class AppUser : IdentityUser
    {
        [Key]
        public int UserId { get; set; }
        public int EmployeeId { get; set; }
        public string UserType { get; set; }
        public bool IsLoggedin { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime Login { get; set; }
        public DateTime LogOut { get; set; }

        public Employee Employee { get; set; }
    }
}
