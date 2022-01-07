using System.Collections.Generic;
using Moonpig.PostOffice.Data;
using Moonpig.PostOffice.Data.Repositories;

namespace Moonpig.PostOffice.Api.Services
{
    public class SupplierService
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IProductRepository _productRepository;
        public SupplierService(ISupplierRepository supplierRepository, IProductRepository productRepository)
        {
            _supplierRepository = supplierRepository;
            _productRepository = productRepository;

        }
        public IEnumerable<Supplier> GetSuppliers(IEnumerable<int> productIds)
        {
            var products = _productRepository.GetProducts(productIds);
            var suppliers = _supplierRepository.GetSuppliers(products);
            return suppliers;
        }
    }
}