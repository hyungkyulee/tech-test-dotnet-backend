using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Moonpig.PostOffice.Api.Model;
using Moonpig.PostOffice.Api.Services;
using Moonpig.PostOffice.Data.Repositories;

namespace Moonpig.PostOffice.Api.Controllers
{
    [Route("api/[controller]")]
    public class DispatchDateController : Controller
    {
        private SupplierService _supplierService;

        // private HolidayService _holidayService;
        // private object _holidayService;

        public DispatchDateController(SupplierService supplierService)
        {
            _supplierService = supplierService;
        }
        
        [HttpGet]
        public DispatchDate Get(IEnumerable<int> productIds, DateTime orderDate)
        {
            var suppliers = _supplierService.GetSuppliers(productIds);
            // var holidays = _holidayService.GetBankHolidays();
            
            // var suppliers = _supplierRepository.GetSuppliersFrom(productIds);

            var postOffice = new Models.Domains.PostOffice(suppliers);
            var responseBody = new DispatchDate { Date = postOffice.GetDispatchDate(orderDate) };
            return responseBody;
        }
    }
    
}
