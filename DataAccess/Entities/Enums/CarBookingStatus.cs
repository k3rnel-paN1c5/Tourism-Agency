namespace DataAccess.Entities.Enums;

public enum CarBookingStatus
{
    Pending,//waiting to get accepted by an employee
    Denied,
    NotStartedYet,//accepted but not started yet
    InProgress,
    Completed,// or Ended
    Cancelled

}
