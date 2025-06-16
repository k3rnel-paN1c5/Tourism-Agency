import './PaymentCard.css'
import { useState, useEffect } from 'react';
import paymentService from '../../services/paymentService';

const PaymentCard = ({ payment, onClick }) => {
  const [paymentDetails, setPaymentDetails] = useState(null);

  const getStatusColor = (status) => {
    switch (status) {
      case "Paid":
        return "payment-status-paid";
      case "Pending":
        return "payment-status-pending";
      case "Cancelled":
        return "payment-status-cancelled";
      case "Refunded":
        return "payment-status-refunded";
      default:
        return "payment-status-default";
    }
  };

  const fetchPaymentDetails = async () => {
    if (!payment?.id) return;
    
    try {
      const details = await paymentService.getPaymentDetails(payment.id);
      setPaymentDetails(details);
    } catch (error) {
      console.log('Could not fetch payment details');
    }
  };

  useEffect(() => {
    fetchPaymentDetails();
  }, [payment?.id]);

  const formatCurrency = (amount) => {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD'
    }).format(amount);
  };

  const formatDate = (dateString) => {
    return new Date(dateString).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    });
  };

  const paymentInfo = {
    "Payment ID": payment.id,
    "Booking ID": payment.bookingId,
    "Amount": formatCurrency(payment.amount),
    "Created": formatDate(payment.createdAt),
    ...(payment.notes && { "Notes": payment.notes })
  };

  return (
    <div className="payment-card" onClick={() => onClick(payment)}>
      <div className="payment-card-header">
        <h3 className="payment-card-title">Payment #{payment.id}</h3>
        <span className={`payment-status ${getStatusColor(payment.status)}`}>
          {payment.status}
        </span>
      </div>
      <ul className="payment-details">
        {Object.entries(paymentInfo).map(([key, value]) => (
          <li key={key} className="payment-detail-item">
            <strong>{key}:</strong> {value || "N/A"}
          </li>
        ))}
      </ul>
      {paymentDetails && paymentDetails.transactionCount > 0 && (
        <div className="transaction-info">
          <span className="label">Transactions:</span>
          <span className="count">{paymentDetails.transactionCount}</span>
        </div>
      )}
    </div>
  );
};

export default PaymentCard;