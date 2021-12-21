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
            var maxBusinessLeadTime = _suppliers
                .Select(s => s.LeadTime)
                .Max();

            return CalculateDispatchDate(orderDate, maxBusinessLeadTime);
        }

        private DateTime CalculateDispatchDate(DateTime orderDate, int businessLeadTime)
        {
            var businessDays = 0;
            var dispatchDate = orderDate;
            while (businessDays++ < businessLeadTime)
            {
                dispatchDate = MoveToNextBusinessDate(dispatchDate);
                Console.WriteLine($"count: {businessDays}, updated dispatchDate: {dispatchDate}");
            }

            return dispatchDate;
        }
        
        private DateTime MoveToNextBusinessDate(DateTime orderDate)
        {
            var startWorkingDate = orderDate.DayOfWeek switch
            {
                DayOfWeek.Saturday => orderDate.AddDays(2),
                DayOfWeek.Sunday => orderDate.AddDays(1),
                _ => orderDate
            };
            
            var nextWorkingDate = startWorkingDate.AddDays(1); 
            return nextWorkingDate.DayOfWeek switch
            {
                DayOfWeek.Saturday => nextWorkingDate.AddDays(2),
                DayOfWeek.Sunday => nextWorkingDate.AddDays(1),
                _ => nextWorkingDate
            };
        }
        
        public PostOffice(IEnumerable<Supplier> suppliers)
        {
            _suppliers = suppliers;
        }
    }
}
