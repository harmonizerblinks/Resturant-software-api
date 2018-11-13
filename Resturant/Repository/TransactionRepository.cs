using Resturant.Models;

namespace Resturant.Repository
{
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
