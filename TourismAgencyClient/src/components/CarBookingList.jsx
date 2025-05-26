import './BookingList.css';

const CarBookingList = ({ bookings }) => {
  if (!bookings || bookings.length === 0) {
    return (
      <div className="empty-state">
        <p>No car bookings found</p>
        <p className="sub-text">Start your journey by booking a car!</p>
      </div>
    );
  }

  const formatDate = (dateString) => {
    return new Date(dateString).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    });
  };

  return (
    <div className="booking-list">
      {bookings.map((booking) => (
        <div key={booking.id} className="booking-card">
          <div className="booking-header">
            <h3>Car Booking #{booking.id}</h3>
            <span className={`status-badge ${booking.status.toLowerCase()}`}>
              {booking.status}
            </span>
          </div>
          
          <div className="booking-details">
            <div className="detail-item">
              <span className="label">Start Date:</span>
              <span className="value">{formatDate(booking.startDate)}</span>
            </div>
            <div className="detail-item">
              <span className="label">End Date:</span>
              <span className="value">{formatDate(booking.endDate)}</span>
            </div>
            <div className="detail-item">
              <span className="label">Passengers:</span>
              <span className="value">{booking.numOfPassengers}</span>
            </div>
            <div className="detail-item">
              <span className="label">Driver:</span>
              <span className="value">{booking.withDriver ? 'Included' : 'Not Included'}</span>
            </div>
            <div className="detail-item">
              <span className="label">Pickup Location:</span>
              <span className="value">{booking.pickUpLocation || 'Not specified'}</span>
            </div>
            <div className="detail-item">
              <span className="label">Dropoff Location:</span>
              <span className="value">{booking.dropOffLocation || 'Not specified'}</span>
            </div>
          </div>

          <div className="booking-actions">
            <button className="action-button view">View Details</button>
            {booking.status === 'Active' && (
              <button className="action-button cancel">Cancel Booking</button>
            )}
          </div>
        </div>
      ))}
    </div>
  );
};

export default CarBookingList; 