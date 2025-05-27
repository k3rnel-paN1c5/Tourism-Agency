using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utilities
{
    public class  CarBookingAmountDueCalculator
    {
        public decimal CalculateAmountDue( DateTime startDate,DateTime endDate, decimal ppd, decimal pph)
        {
            var duration = endDate - startDate;
            var hours = duration.TotalHours;
            decimal AmountDue = 0;
            if(hours>=24)
            {
                var days = (int)duration.TotalDays;
                AmountDue += days * ppd;
                hours = hours - days * 24;
            }
            AmountDue += ((int)hours) * pph;
            return AmountDue;

                                  
        
        }
    }
}
