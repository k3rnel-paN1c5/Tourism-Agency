import './BookingCard.css'

const BookingCard = ({ title, details, status }) => {
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
    </div>
  );
};
export default BookingCard;