import React from 'react';
import TripBookingCard from './TripBookingCard';
import './BookingList.css';

export default function TripBookingList({ bookings }) {
  if (!bookings || bookings.length === 0) {
    return (
      <div className="booking-list empty">
        <p>No trip bookings found</p>
      </div>
    );
  }

  return (
    <div className="booking-list">
      <h2>Trip Bookings</h2>
      <div className="booking-grid">
        {bookings.map((booking) => (
          <TripBookingCard key={booking.id} booking={booking} />
        ))}
      </div>
    </div>
  );
} 