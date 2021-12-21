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

            var dispatchDate = orderDate.AddDays(GetActualLeadTime(orderDate, maxBusinessLeadTime));
            return dispatchDate;
        }

        private static int GetActualLeadTime(DateTime orderDate, int businessLeadTime)
        {
            var actualDays = 0;
            while (businessLeadTime-- > 0)
            {
                switch (orderDate.DayOfWeek)
                {
                    case DayOfWeek.Saturday:
                        actualDays += 3;
                        break;
                    case DayOfWeek.Sunday:
                        actualDays += 2;
                        break;
                    default:
                    {
                        actualDays += 1;
                        switch (orderDate.AddDays(actualDays).DayOfWeek)
                        {
                            case DayOfWeek.Saturday:
                                actualDays += 2;
                                break;
                            case DayOfWeek.Sunday:
                                actualDays += 1;
                                break;
                        }
                        break;
                    }
                }

                Console.WriteLine($"count: {actualDays}");
            }

            return actualDays;
        }

        public PostOffice(IEnumerable<Supplier> suppliers)
        {
            _suppliers = suppliers;
        }
    }
}
