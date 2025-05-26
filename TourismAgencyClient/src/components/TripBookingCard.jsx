import React from 'react';
import './TripBookingCard.css';

export default function TripBookingCard({ booking }) {
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
        <h3>Trip Booking #{booking.id}</h3>
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
          <span className="label">Guide:</span>
          <span className="value">{booking.withGuide ? 'Included' : 'Not Included'}</span>
        </div>
        
        <div className="detail-row">
          <span className="label">Trip Plan ID:</span>
          <span className="value">{booking.tripPlanId}</span>
        </div>
      </div>
    </div>
  );
} 