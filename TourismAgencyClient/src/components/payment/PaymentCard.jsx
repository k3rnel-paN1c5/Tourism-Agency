import { formatCurrency, formatDate } from '../../utils/formatters';
import './PaymentCard.css';

export default function PaymentCard({ payment, onAction }) {
  const getStatusClass = (status) => {
    switch (status?.toLowerCase()) {
      case 'paid': return 'status-paid';
      case 'pending': return 'status-pending';
      case 'cancelled': return 'status-cancelled';
      case 'refunded': return 'status-refunded';
      default: return 'status-default';
    }
  };

  const canProcess = payment.status === 'Pending';
  const canRefund = payment.status === 'Paid';

  return (
    <div className="payment-card">
      <div className="payment-header">
        <div className="payment-id">Payment #{payment.id}</div>
        <div className={`payment-status ${getStatusClass(payment.status)}`}>
          {payment.status}
        </div>
      </div>

      <div className="payment-details">
        <div className="detail-row">
          <span className="label">Booking ID:</span>
          <span className="value">{payment.bookingId}</span>
        </div>
        <div className="detail-row">
          <span className="label">Amount:</span>
          <span className="value amount">{formatCurrency(payment.amount)}</span>
        </div>
        <div className="detail-row">
          <span className="label">Created:</span>
          <span className="value">{formatDate(payment.createdAt)}</span>
        </div>
        {payment.notes && (
          <div className="detail-row">
            <span className="label">Notes:</span>
            <span className="value notes">{payment.notes}</span>
          </div>
        )}
      </div>

      <div className="payment-actions">
        <button
          className="btn btn-secondary"
          onClick={() => onAction(payment, 'details')}
        >
          View Details
        </button>
        
        {canProcess && (
          <button
            className="btn btn-primary"
            onClick={() => onAction(payment, 'process')}
          >
            Process Payment
          </button>
        )}
        
        {canRefund && (
          <button
            className="btn btn-warning"
            onClick={() => onAction(payment, 'refund')}
          >
            Process Refund
          </button>
        )}
      </div>
    </div>
  );
}