using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.Utilities
{
    /// <summary>
    /// Provides utility methods for calculating the amount due for car bookings.
    /// This class handles calculations based on start date, end date, price per day (PPD), and price per hour (PPH).
    /// </summary>
    public class  CarBookingAmountDueCalculator
    {

        /// <summary>
        /// Calculates the total amount due for a car booking based on its duration and pricing.
        /// The calculation prioritizes full days and then adds remaining hours.
        /// </summary>
        /// <param name="startDate">The start date and time of the car booking.</param>
        /// <param name="endDate">The end date and time of the car booking.</param>
        /// <param name="ppd">The price per day for the car.</param>
        /// <param name="pph">The price per hour for the car.</param>
        /// <returns>The total decimal amount due for the car booking.</returns>
        public static decimal CalculateAmountDue( DateTime startDate,DateTime endDate, decimal ppd, decimal pph)
        {
            var duration = endDate - startDate;
            var hours = duration.TotalHours;
            decimal AmountDue = 0;

            // If the booking duration is 24 hours or more, calculate full days first.
            if (hours>=24)
            {
                // Get the number of full days.
                var days = (int)duration.TotalDays;
                // Add the cost for full days.
                AmountDue += days * ppd;
                // Subtract the hours accounted for by full days from the total hours.
                hours -= days * 24;
            }
            // Add the cost for any remaining partial hours (rounded down to the nearest whole hour).
            AmountDue += ((int)hours) * pph;
            return AmountDue;

        }
    }
}
