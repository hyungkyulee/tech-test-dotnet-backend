using System;
using System.Collections.Generic;
using System.Linq;
using Moonpig.PostOffice.Data;

namespace Moonpig.PostOffice.Api.Models.Domains
{
    public class PostOffice
    {
        private readonly IEnumerable<Supplier> _suppliers;

        public DateTime GetDispatchDate(DateTime orderDate)
        {
            var maxSuppliedDate = orderDate.AddDays(_suppliers
                .Select(s => s.LeadTime)
                .ToList()
                .Max());

            return maxSuppliedDate.DayOfWeek switch
            {
                DayOfWeek.Saturday => maxSuppliedDate.AddDays(2),
                DayOfWeek.Sunday => maxSuppliedDate.AddDays(1),
                _ => maxSuppliedDate
            };
        }

        public PostOffice(IEnumerable<Supplier> suppliers)
        {
            _suppliers = suppliers;
        }
    }
}
