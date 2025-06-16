import { useState } from 'react';
import paymentService from '../../services/paymentService';

export default function PaymentCreationModal({ booking, onClose, onSuccess }) {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const handleCreatePayment = async () => {
    try {
      setLoading(true);
      const paymentData = {
        bookingId: booking.id,
        amount: booking.totalAmount || booking.price,
        notes: `Payment for ${booking.type} booking #${booking.id}`
      };
      
      const newPayment = await paymentService.createPayment(paymentData);
      onSuccess(newPayment);
    } catch (error) {
      setError(error.response?.data?.Error || 'Failed to create payment');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="modal-overlay">
      <div className="modal-content">
        <div className="modal-header">
          <h3>Create Payment</h3>
          <button className="close-btn" onClick={onClose}>&times;</button>
        </div>
        <div className="modal-body">
          {error && <div className="error-message">{error}</div>}
          <div className="payment-summary">
            <p><strong>Booking ID:</strong> {booking.id}</p>
            <p><strong>Amount:</strong> ${booking.totalAmount || booking.price}</p>
          </div>
          <div className="form-actions">
            <button onClick={onClose} className="btn btn-secondary">Cancel</button>
            <button 
              onClick={handleCreatePayment} 
              disabled={loading}
              className="btn btn-primary"
            >
              {loading ? 'Creating...' : 'Create Payment'}
            </button>
          </div>
        </div>
      </div>
    </div>
  );
}