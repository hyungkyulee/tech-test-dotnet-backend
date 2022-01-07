using System.Collections.Generic;
using System.Linq;

namespace Moonpig.PostOffice.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDbContext _dbContext;
        public ProductRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public IEnumerable<Product> GetProducts(IEnumerable<int> productIds)
        {
            return productIds?.Select(p => _dbContext.Products.SingleOrDefault(x => x.ProductId == p));
        }
    }
}