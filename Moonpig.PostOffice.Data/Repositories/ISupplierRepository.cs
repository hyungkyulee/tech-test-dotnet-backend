using System.Collections.Generic;

namespace Moonpig.PostOffice.Data.Repositories
{
    public interface ISupplierRepository
    {
        IEnumerable<Supplier> GetSuppliersFrom(IEnumerable<int> productIds);
    }
}