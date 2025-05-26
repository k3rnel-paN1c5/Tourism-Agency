import React from 'react';
import './CarBookingCard.css';

export default function CarBookingCard({ booking }) {
  const formatDate = (date) => {
    return new Date(date).toLocaleDateString();
  };

  const getStatusColor = (status) => {
    switch (status.toLowerCase()) {
      case 'confirmed':
        return '#4CAF50';
      case 'pending':
        return '#FFC107';
      case 'cancelled':
        return '#F44336';
      default:
        return '#9E9E9E';
    }
  };

  return (
    <div className="booking-card">
      <div className="booking-header">
        <h3>Car Booking #{booking.id}</h3>
        <span 
          className="status-badge"
          style={{ backgroundColor: getStatusColor(booking.status) }}
        >
          {booking.status}
        </span>
      </div>
      
      <div className="booking-details">
        <div className="detail-row">
          <span className="label">Dates:</span>
          <span className="value">
            {formatDate(booking.startDate)} - {formatDate(booking.endDate)}
          </span>
        </div>
        
        <div className="detail-row">
          <span className="label">Passengers:</span>
          <span className="value">{booking.numOfPassengers}</span>
        </div>
        
        <div className="detail-row">
          <span className="label">Driver:</span>
          <span className="value">{booking.withDriver ? 'Included' : 'Not Included'}</span>
        </div>
        
        <div className="detail-row">
          <span className="label">Car ID:</span>
          <span className="value">{booking.carId}</span>
        </div>

        <div className="detail-row">
          <span className="label">Pickup:</span>
          <span className="value">{booking.pickUpLocation}</span>
        </div>

        <div className="detail-row">
          <span className="label">Dropoff:</span>
          <span className="value">{booking.dropOffLocation}</span>
        </div>
      </div>
    </div>
  );
} 