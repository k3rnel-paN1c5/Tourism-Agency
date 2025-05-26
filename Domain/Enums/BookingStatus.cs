namespace Domain.Enums;

public enum BookingStatus
{
    Pending,//waiting to get accepted by an employee
    Denied,
    Confirmed,//accepted but not started yet
    InProgress,
    Completed,// or Ended
    Cancelled

}