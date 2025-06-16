import { useState } from 'react';
import PaymentCard from './PaymentCard';
import PaymentModal from './PaymentModal';
import './PaymentSection.css';

export default function PaymentSection({ payments, onPaymentUpdate }) {
  const [selectedPayment, setSelectedPayment] = useState(null);
  const [showModal, setShowModal] = useState(false);
  const [modalType, setModalType] = useState(''); // 'process', 'refund', 'details'

  const handlePaymentAction = (payment, action) => {
    setSelectedPayment(payment);
    setModalType(action);
    setShowModal(true);
  };

  const handleCloseModal = () => {
    setShowModal(false);
    setSelectedPayment(null);
    setModalType('');
  };

  const handlePaymentSuccess = (updatedPayment) => {
    onPaymentUpdate(updatedPayment);
    handleCloseModal();
  };

  return (
    <div className="payment-section">
      <div className="section-header">
        <h2>Payment Management</h2>
        <p>Manage your payments and transactions</p>
      </div>

      <div className="payments-grid">
        {payments.length === 0 ? (
          <div className="no-payments">
            <p>No payments found</p>
          </div>
        ) : (
          payments.map(payment => (
            <PaymentCard
              key={payment.id}
              payment={payment}
              onAction={handlePaymentAction}
            />
          ))
        )}
      </div>

      {showModal && (
        <PaymentModal
          payment={selectedPayment}
          type={modalType}
          onClose={handleCloseModal}
          onSuccess={handlePaymentSuccess}
        />
      )}
    </div>
  );
}