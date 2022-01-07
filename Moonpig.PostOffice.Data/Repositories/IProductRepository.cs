using System.Collections.Generic;

namespace Moonpig.PostOffice.Data.Repositories
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetProducts(IEnumerable<int> productIds);
    }
}