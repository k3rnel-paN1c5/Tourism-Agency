import './PaymentCard.css'

const PaymentCard = ({ payment, onClick }) => {
  // Convert numeric status to text
  const getStatusText = (statusCode) => {
    switch (statusCode) {
      case 0:
        return "Pending";
      case 1:
        return "Paid";
      case 2:
        return "Cancelled";
      case 3:
        return "Refunded";
      case 4:
        return "PartiallyPaid";
      default:
        return "Unknown";
    }
  };
  
  const status = getStatusText(payment?.status);

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
      case "PartiallyPaid":
        return "payment-status-partial";
      default:
        return "payment-status-default";
    }
  };

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
    "Amount Due": formatCurrency(payment.amountDue),
    "Amount Paid": formatCurrency(payment.amountPaid || 0),
    "Payment Date": payment.paymentDate ? formatDate(payment.paymentDate) : "Not paid",
    "Created": formatDate(payment.createdAt),
    ...(payment.notes && { "Notes": payment.notes })
  };

  return (
    <div className="payment-card" onClick={() => onClick(payment)}>
      <div className="payment-card-header">
        <h3 className="payment-card-title">Payment #{payment.id}</h3>
        <span className={`payment-status ${getStatusColor(status)}`}>{status}</span>
      </div>
      <ul className="payment-details">
        {Object.entries(paymentInfo).map(([key, value]) => (
          <li key={key} className="payment-detail-item">
            <strong>{key}:</strong> {value || "N/A"}
          </li>
        ))}
      </ul>
    </div>
  );
};

export default PaymentCard;