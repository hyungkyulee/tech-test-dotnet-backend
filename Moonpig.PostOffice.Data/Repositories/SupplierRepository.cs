using System.Collections.Generic;
using System.Linq;

namespace Moonpig.PostOffice.Data.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly IDbContext _dbContext;
        public SupplierRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public IEnumerable<Supplier> GetSuppliersFrom(IEnumerable<int> productIds)
        {
            var suppliers = new List<Supplier>();
            
            foreach (var productId in productIds)
            {
                var product =  _dbContext.Products.SingleOrDefault(p => p.ProductId == productId);
                if (product == null) continue;

                var supplier = _dbContext.Suppliers.SingleOrDefault(s => s.SupplierId == product.SupplierId);
                if (supplier == null) continue;
                
                suppliers.Add(supplier);    
            }

            return suppliers;
        }
    }
}
