import Section from "../shared/Section";
import BookingCard from "./BookingCard";
import  './Booking.css' ;

const CarBookingsSection = ({ bookings }) => {
  let cnt = 1, noBooking = false;
  return (
    <>
    <Section title="Car Bookings">
      {bookings.length > 0 ? (
        bookings.map((booking) => (
          <BookingCard
            key={booking.id}
            title={`Car #${cnt++}`}
            status= {booking.status == 0 ? 'Pending' : booking.status == 2 ? 'Confirmed': booking.status == 5 ? 'Canceled' :booking.status}
            details={{
              "Start Date": new Date(booking.startDate).toLocaleDateString(),
              "End Date": new Date(booking.endDate).toLocaleDateString(),
              "Passengers": booking.numOfPassengers,
              "Pickup Location": booking.pickUpLocation,
              "Dropoff Location": booking.dropOffLocation,
              "With Driver": booking.withDriver ? "Yes" : "No"
            }}
          />
        ))
      ) : (
        <p></p>
      )}
    </Section>
    {bookings.length == 0 ? <p className="no-bookings">Loading or no car bookings found.</p> : <p></p>}
    </>
  );
};

export default CarBookingsSection;
