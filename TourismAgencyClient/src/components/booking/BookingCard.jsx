import './BookingCard.css'
import { useState } from 'react';
import paymentService from '../../services/paymentService';

const BookingCard = ({ title, details, status }) => {
  const [paymentInfo, setPaymentInfo] = useState(null);

  const getStatusColor = (status) => {
    switch (status) {
      case "Confirmed":
        return "booking-status-confirmed";
      case "Pending":
        return "booking-status-pending";
      case "Canceled":
        return "booking-status-canceled";
      default:
        return "booking-status-default";
    }
  };

  const fetchPaymentInfo = async () => {
    try {
      const payment = await paymentService.getPaymentByBookingId(booking.id);
      setPaymentInfo(payment);
    } catch (error) {
      console.log('No payment found for this booking');
    }
  };

  return (
    <div className="booking-card">
      <div className="booking-card-header">
        <h3 className="booking-card-title">{title}</h3>
        <span className={`booking-status ${getStatusColor(status)}`}>{status}</span>
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