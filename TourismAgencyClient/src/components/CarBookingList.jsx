import React from 'react';
import CarBookingCard from './CarBookingCard';
import './BookingList.css';

export default function CarBookingList({ bookings }) {
  if (!bookings || bookings.length === 0) {
    return (
      <div className="booking-list empty">
        <p>No car bookings found</p>
      </div>
    );
  }

  return (
    <div className="booking-list">
      <h2>Car Bookings</h2>
      <div className="booking-grid">
        {bookings.map((booking) => (
          <CarBookingCard key={booking.id} booking={booking} />
        ))}
      </div>
    </div>
  );
} 