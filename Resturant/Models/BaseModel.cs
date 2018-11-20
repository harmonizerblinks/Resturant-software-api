using System;

namespace Resturant.Models
{
    public class BaseModel
    {
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public string MUserId { get; set; }
        public DateTime? MDate { get; set; }

        //[ForeignKey("UserId")]
        //public AppUser User { get; set; }
    }
}
