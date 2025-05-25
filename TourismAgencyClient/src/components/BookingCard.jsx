import React from 'react';
import './BookingCard.css';

export default function BookingCard({ 
  title, 
  status, 
  details, 
  type = 'trip' // 'trip' or 'car'
}) {
  return (
    <div className={`booking-card ${type}`}>
      <div className="booking-header">
        <h3>{title}</h3>
        <span className={`status ${status.toLowerCase()}`}>{status}</span>
      </div>
      <div className="booking-details">
        {Object.entries(details).map(([key, value]) => (
          <p key={key}>
            <strong>{key}:</strong> {value}
          </p>
        ))}
      </div>
    </div>
  );
} 