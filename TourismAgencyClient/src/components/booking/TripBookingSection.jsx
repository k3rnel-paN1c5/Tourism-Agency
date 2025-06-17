import Section from "../shared/Section";
import BookingCard from "./BookingCard";
import  './Booking.css' ;

const TripBookingsSection = ({ bookings, onBookingClick }) => {
  let cnt = 1;
  console.log(bookings);
  bookings = bookings.filter(b => b.status != 5);
  return (
    <>
    <Section title="Trip Bookings">
      {bookings.length > 0 ? (
        bookings.map((booking) => (
          <BookingCard
            key={booking.id}
            title={`Trip #${cnt++}`}
            status= {booking.status == 0 ? 'Pending' : booking.status == 2 ? 'Confirmed': booking.status == 5 ? 'Canceled' :booking.status}
            details={{
              "Start Date": new Date(booking.startDate).toLocaleDateString(),
              "End Date": new Date(booking.endDate).toLocaleDateString(),
              "Passengers": booking.numOfPassengers,
              "With Guide": booking.withGuide ? "Yes" : "No",
            }}
            onClick={() => onBookingClick(booking)}
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
