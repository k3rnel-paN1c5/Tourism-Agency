using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utilities;

public static class TripBookingAmountDueCalculator
{
    /// <summary>
    /// Calculates the total amount due for a trip booking based on seat price and number of passengers.
    /// </summary>
    /// <param name="seatPrice">The price per seat for the trip.</param>
    /// <param name="NumberOfPassengers">The total number of passengers for the booking.</param>
    /// <returns>The total amount due for the trip booking.</returns>
    public static decimal CalculateAmountDue(decimal seatPrice, int NumberOfPassengers)
    {
        decimal AmountDue = seatPrice * NumberOfPassengers;
        return AmountDue;

    }
}
