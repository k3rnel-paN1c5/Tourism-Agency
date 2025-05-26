import Section from "./Section";
import BookingCard from "./BookingCard";
import  './Booking.css' ;

const TripBookingsSection = ({ bookings }) => {
  let cnt = 1;
  console.log(bookings);
  return (
    <>
    <Section title="Trip Bookings">
      {bookings.length > 0 ? (
        bookings.map((booking) => (
          <BookingCard
            key={booking.id}
            title={`Trip #${cnt++}`}
            status= {booking.status == 0 ? 'Pending' : booking.status == 2 ? 'Confirmed': 'Canceled'}
            details={{
              "Start Date": new Date(booking.startDate).toLocaleDateString(),
              "End Date": new Date(booking.endDate).toLocaleDateString(),
              "Passengers": booking.numOfPassengers,
              "With Guide": booking.withGuide ? "Yes" : "No",
              "Employee Assigned": booking.employeeId || "None"
            }}
          />
        ))
      ) : (
        <p></p>
      )}
    </Section>
    {bookings.length == 0 ? <p className="no-bookings">Loading or no trip bookings found.</p> : <p></p>}
    </>
  );
};

export default TripBookingsSection;
