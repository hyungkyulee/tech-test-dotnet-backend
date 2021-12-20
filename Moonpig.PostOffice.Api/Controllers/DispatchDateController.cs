using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moonpig.PostOffice.Api.Model;
using Moonpig.PostOffice.Data.Repositories;

namespace Moonpig.PostOffice.Api.Controllers
{
    [Route("api/[controller]")]
    public class DispatchDateController : Controller
    {
        private readonly ISupplierRepository _supplierRepository;

        public DispatchDateController(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }
        
        [HttpGet]
        public DispatchDate Get(IEnumerable<int> productIds, DateTime orderDate)
        {
            var suppliers = _supplierRepository.GetSuppliersFrom(productIds);

            var postOffice = new Models.Domains.PostOffice(suppliers);
            var responseBody = new DispatchDate { Date = postOffice.GetDispatchDate(orderDate) };
            return responseBody;
        }
    }
}
