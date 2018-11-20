using Resturant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resturant.Repository
{
    public class SmsRepository : GenericRepository<Sms>, ISmsRepository
    {
        public SmsRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
