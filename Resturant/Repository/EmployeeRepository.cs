using Resturant.Models;

namespace Resturant.Repository
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
