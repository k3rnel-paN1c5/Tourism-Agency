import './BookingCard.css'
import { useState, useEffect } from 'react';
import paymentService from '../../services/paymentService';

const BookingCard = ({ title, details, status, onClick, bookingId }) => {
  const [paymentInfo, setPaymentInfo] = useState(null);

  const getStatusColor = (status) => {

    switch (status) {
      case 2:
        return "booking-status-confirmed";
      case 0:
        return "booking-status-pending";
      case 5:
        return "booking-status-canceled";
      default:
        return "booking-status-default";
    }
  };

  const fetchPaymentInfo = async () => {
    if (!bookingId) return;
    
    try {
      const payment = await paymentService.getPaymentByBookingId(bookingId);
      setPaymentInfo(payment);
    } catch (error) {
      console.log('No payment found for this booking');
    }
  };

  useEffect(() => {
    fetchPaymentInfo();
  }, [bookingId]);

  return (
    <div className="booking-card" onClick={onClick}>
      <div className="booking-card-header">
        <h3 className="booking-card-title">{title}</h3>
        <span className={`booking-status ${getStatusColor(status)}`}>{status }</span>
      </div>
      <ul className="booking-details">
        {Object.entries(details).map(([key, value]) => (
          <li key={key} className="booking-detail-item">
            <strong>{key}:</strong> {value || "N/A"}
          </li>
        ))}
      </ul>
      {paymentInfo && (
        <div className="payment-status">
          <span className="label">Payment Status:</span>
          <span className={`status ${paymentInfo.status.toLowerCase()}`}>
            {paymentInfo.status}
          </span>
        </div>
      )}
    </div>
  );
};

export default BookingCard;