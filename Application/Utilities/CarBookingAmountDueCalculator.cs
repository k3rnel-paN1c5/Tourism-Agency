using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.Utilities
{
<<<<<<< HEAD
    public static class CarBookingAmountDueCalculator
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
=======
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

        /// <summary>
        /// Calculates the total amount due for a car booking based on its duration and pricing.
        /// The calculation prioritizes full days and then adds remaining hours.
        /// </summary>
        /// <param name="startDate">The start date and time of the car booking.</param>
        /// <param name="endDate">The end date and time of the car booking.</param>
        /// <param name="ppd">The price per day for the car.</param>
        /// <param name="pph">The price per hour for the car.</param>
        /// <returns>The total decimal amount due for the car booking.</returns>
        public decimal CalculateAmountDue( DateTime startDate,DateTime endDate, decimal ppd, decimal pph)
>>>>>>> eb412ad (Docs for mapping profiles related to car booking)
        {
            var duration = endDate - startDate;
            var hours = duration.TotalHours;
            decimal AmountDue = 0;
<<<<<<< HEAD
            if (hours >= 24)
=======

            // If the booking duration is 24 hours or more, calculate full days first.
            if (hours>=24)
>>>>>>> eb412ad (Docs for mapping profiles related to car booking)
            {
                // Get the number of full days.
                // Get the number of full days.
                var days = (int)duration.TotalDays;
                // Add the cost for full days.
                // Add the cost for full days.
                AmountDue += days * ppd;
<<<<<<< HEAD
                hours -= days * 24;
=======
                // Subtract the hours accounted for by full days from the total hours.
                hours = hours - days * 24;
>>>>>>> eb412ad (Docs for mapping profiles related to car booking)
            }
            // Add the cost for any remaining partial hours (rounded down to the nearest whole hour).
            // Add the cost for any remaining partial hours (rounded down to the nearest whole hour).
            AmountDue += ((int)hours) * pph;
            return AmountDue;

        }
    }
}
