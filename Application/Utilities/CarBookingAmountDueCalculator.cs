using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.Utilities
{
    public static class CarBookingAmountDueCalculator
    /// <summary>
    /// Provides utility methods for calculating the amount due for car bookings.
    /// This class handles calculations based on start date, end date, price per day (PPD), and price per hour (PPH).
    /// </summary>
    public class  CarBookingAmountDueCalculator
    {
        /// <summary>
        /// Calculates the total amount due for a car booking based on per-day and per-hour rates.
        /// </summary>
        /// <param name="startDate">The start date and time of the car booking.</param>
        /// <param name="endDate">The end date and time of the car booking.</param>
        /// <param name="ppd">Price per day for the car rental.</param>
        /// <param name="pph">Price per hour for the car rental.</param>
        /// <returns>The total amount due for the car booking, calculated with daily and hourly rates.</returns>
        public static decimal CalculateAmountDue(DateTime startDate, DateTime endDate, decimal ppd, decimal pph)
        {
            var duration = endDate - startDate;
            var hours = duration.TotalHours;
            decimal AmountDue = 0;
            if (hours >= 24)

            // If the booking duration is 24 hours or more, calculate full days first.
            if (hours>=24)
            {
                // Get the number of full days.
                var days = (int)duration.TotalDays;
                // Add the cost for full days.
                AmountDue += days * ppd;
                hours -= days * 24;
                // Subtract the hours accounted for by full days from the total hours.
                hours = hours - days * 24;
            }
            // Add the cost for any remaining partial hours (rounded down to the nearest whole hour).
            AmountDue += ((int)hours) * pph;
            return AmountDue;

        }
    }
}
