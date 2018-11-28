using Resturant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resturant.Repository
{
    public class TransitRepository : GenericRepository<Transit>, ITransitRepository
    {
        public TransitRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
