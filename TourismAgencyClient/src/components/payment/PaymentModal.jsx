import { useState, useEffect } from 'react';
import paymentService from '../../services/paymentService';
import './PaymentModal.css';

export default function PaymentModal({ payment, type, onClose, onSuccess }) {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [paymentDetails, setPaymentDetails] = useState(null);
  const [formData, setFormData] = useState({
    amount: payment?.amount || 0,
    paymentMethod: 'CreditCard',
    reason: '',
    notes: ''
  });

  useEffect(() => {
    if (type === 'details') {
      fetchPaymentDetails();
    }
  }, [type, payment?.id]);

  const fetchPaymentDetails = async () => {
    try {
      setLoading(true);
      const details = await paymentService.getPaymentDetails(payment.id);
      setPaymentDetails(details);
    } catch (error) {
      setError('Failed to load payment details');
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError(null);

    try {
      let result;
      if (type === 'process') {
        result = await paymentService.processPayment(payment.id, {
          paymentId: payment.id,
          paymentMethod: formData.paymentMethod,
          notes: formData.notes
        });
      } else if (type === 'refund') {
        result = await paymentService.processRefund(payment.id, {
          paymentId: payment.id,
          amount: parseFloat(formData.amount),
          reason: formData.reason,
          refundMethod: formData.paymentMethod
        });
      }
      
      onSuccess(result?.payment || result);
    } catch (error) {
      setError(error.response?.data?.Error || error.message || 'Operation failed');
    } finally {
      setLoading(false);
    }
  };

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
  };

  const renderContent = () => {
    if (type === 'details') {
      return (
        <div className="payment-details-content">
          {loading ? (
            <div className="loading">Loading payment details...</div>
          ) : paymentDetails ? (
            <div className="details-grid">
              <div className="detail-item">
                <label>Payment ID:</label>
                <span>{paymentDetails.id}</span>
              </div>
              <div className="detail-item">
                <label>Booking ID:</label>
                <span>{paymentDetails.bookingId}</span>
              </div>
              <div className="detail-item">
                <label>Amount:</label>
                <span>${paymentDetails.amount}</span>
              </div>
              <div className="detail-item">
                <label>Status:</label>
                <span>{paymentDetails.status}</span>
              </div>
              <div className="detail-item">
                <label>Created:</label>
                <span>{new Date(paymentDetails.createdAt).toLocaleString()}</span>
              </div>
              {paymentDetails.notes && (
                <div className="detail-item full-width">
                  <label>Notes:</label>
                  <span>{paymentDetails.notes}</span>
                </div>
              )}
            </div>
          ) : (
            <div className="error">Failed to load payment details</div>
          )}
        </div>
      );
    }

    return (
      <form onSubmit={handleSubmit} className="payment-form">
        {type === 'refund' && (
          <>
            <div className="form-group">
              <label htmlFor="amount">Refund Amount:</label>
              <input
                type="number"
                id="amount"
                name="amount"
                value={formData.amount}
                onChange={handleInputChange}
                max={payment.amount}
                step="0.01"
                required
              />
            </div>
            <div className="form-group">
              <label htmlFor="reason">Refund Reason:</label>
              <textarea
                id="reason"
                name="reason"
                value={formData.reason}
                onChange={handleInputChange}
                required
                rows="3"
              />
            </div>
          </>
        )}

        <div className="form-group">
          <label htmlFor="paymentMethod">Payment Method:</label>
          <select
            id="paymentMethod"
            name="paymentMethod"
            value={formData.paymentMethod}
            onChange={handleInputChange}
            required
          >
            <option value="CreditCard">Credit Card</option>
            <option value="DebitCard">Debit Card</option>
            <option value="PayPal">PayPal</option>
            <option value="BankTransfer">Bank Transfer</option>
          </select>
        </div>

        <div className="form-group">
          <label htmlFor="notes">Notes (Optional):</label>
          <textarea
            id="notes"
            name="notes"
            value={formData.notes}
            onChange={handleInputChange}
            rows="2"
          />
        </div>

        <div className="form-actions">
          <button type="button" onClick={onClose} className="btn btn-secondary">
            Cancel
          </button>
          <button type="submit" disabled={loading} className="btn btn-primary">
            {loading ? 'Processing...' : type === 'process' ? 'Process Payment' : 'Process Refund'}
          </button>
        </div>
      </form>
    );
  };

  return (
    <div className="modal-overlay">
      <div className="modal-content">
        <div className="modal-header">
          <h3>
            {type === 'details' && 'Payment Details'}
            {type === 'process' && 'Process Payment'}
            {type === 'refund' && 'Process Refund'}
          </h3>
          <button className="close-btn" onClick={onClose}>&times;</button>
        </div>

        <div className="modal-body">
          {error && <div className="error-message">{error}</div>}
          {renderContent()}
        </div>
      </div>
    </div>
  );
}