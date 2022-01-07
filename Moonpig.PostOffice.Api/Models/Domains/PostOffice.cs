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

            var expectedDispatchDate = GetExpectedDispatchDate(orderDate, maxBusinessLeadTime);
            
            // var dispatchDate = orderDate.AddDays(GetActualLeadTime(orderDate, maxBusinessLeadTime));
            //
            // var bankHolidays = GetBankHolidaysBetween(orderDate, dispatchDate);
            //
            // var updatedDispatchDate = dispatchDate.AddDays(bankHolidays);
            
            return ShiftWeekendDispatchDateToNextWorkingDay(expectedDispatchDate);
        }
        private DateTime GetExpectedDispatchDate(DateTime orderDate, int maxBusinessLeadTime)
        {
            var weeks = maxBusinessLeadTime / 5;
            var daysInMultipleWeeks = weeks * 7;
            var surplusDays = maxBusinessLeadTime % 5;

            var fromDate = orderDate.AddDays(daysInMultipleWeeks);
            fromDate = ShiftWeekendDispatchDateToNextWorkingDay(fromDate);
            var toDate = fromDate.AddDays(surplusDays);
            var additionalWeekendDays = Enumerable
                .Range(1, toDate.Subtract(fromDate).Days)
                .Select(n => fromDate.AddDays(n))
                .Count(x => x.DayOfWeek == DayOfWeek.Saturday || x.DayOfWeek == DayOfWeek.Sunday);

            return toDate.AddDays(additionalWeekendDays);
        }
        private DateTime ShiftWeekendDispatchDateToNextWorkingDay(DateTime currentDate)
        {
            return currentDate.DayOfWeek switch
            {
                DayOfWeek.Saturday => currentDate.AddDays(2),
                DayOfWeek.Sunday => currentDate.AddDays(1),
                _ => currentDate
            };
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
        
        private static DateTime[] BankHolidays = new[]
        {
            new DateTime(2022, 1, 3),
            new DateTime(2022, 4, 15),
            new DateTime(2022, 4, 18),
            new DateTime(2022, 5, 2),
            new DateTime(2022, 6, 2),
            new DateTime(2022, 6, 3),
            new DateTime(2022, 8, 29),
            new DateTime(2022, 12, 26),
            new DateTime(2022, 12, 27),
        };
    }
}
