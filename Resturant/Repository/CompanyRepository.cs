using Resturant.Models;

namespace Resturant.Repository
{
    public class CompanyRepository : GenericRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
